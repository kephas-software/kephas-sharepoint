// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharePointMetadataCache.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the SharePoint metadata cache class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Reflection
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Services;
    using Kephas.SharePoint;
    using Kephas.Threading.Tasks;
    using Microsoft.SharePoint.Client;

    /// <summary>
    /// A SharePoint metadata cache.
    /// </summary>
    [OverridePriority(Priority.Low)]
    public class SharePointMetadataCache : ISharePointMetadataCache
    {
        private readonly IListService libraryService;
        private readonly ISiteServiceProvider siteServiceProvider;
        private readonly ConcurrentDictionary<ListIdentity, IListInfo> listInfos = new ConcurrentDictionary<ListIdentity, IListInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointMetadataCache"/> class.
        /// </summary>
        /// <param name="libraryService">The library service.</param>
        /// <param name="siteServiceProvider">The site service provider.</param>
        public SharePointMetadataCache(IListService libraryService, ISiteServiceProvider siteServiceProvider)
        {
            this.libraryService = libraryService;
            this.siteServiceProvider = siteServiceProvider;
        }

        /// <summary>
        /// Clears the cache to its blank/initial state.
        /// </summary>
        public void Clear()
        {
            this.listInfos.Clear();
        }

        /// <summary>
        /// Gets the list type information asynchronously.
        /// </summary>
        /// <param name="listFullName">Full name of the list.</param>
        /// <param name="cancellationToken">Optional. a token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result that yields the list type information.
        /// </returns>
        public async Task<IListInfo> GetListInfoAsync(string listFullName, CancellationToken cancellationToken = default)
        {
            var key = new ListIdentity(listFullName);
            this.listInfos.TryGetValue(key, out var typeInfo);
            if (typeInfo != null)
            {
                return typeInfo;
            }

            var (siteName, listName) = this.libraryService.GetListPathFragments(listFullName);
            var siteService = this.siteServiceProvider.GetSiteService(siteName);
            var list = await siteService.GetListAsync(listFullName).PreserveThreadContext();

            return await this.GetOrUpdateListInfoAsync(key, siteService, list).PreserveThreadContext();
        }

        /// <summary>
        /// Gets the list type information asynchronously.
        /// </summary>
        /// <param name="siteId">Identifier for the site.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="cancellationToken">Optional. a token that allows processing to be
        ///                                 cancelled.</param>
        /// <returns>
        /// An asynchronous result that yields the list type information.
        /// </returns>
        public async Task<IListInfo> GetListInfoAsync(Guid siteId, string listName, CancellationToken cancellationToken = default)
        {
            var key = new ListIdentity(siteId, listName);
            this.listInfos.TryGetValue(key, out var typeInfo);
            if (typeInfo != null)
            {
                return typeInfo;
            }

            var siteService = this.siteServiceProvider.GetSiteService(siteId);
            var list = await siteService.GetListAsync(listName).PreserveThreadContext();

            return await this.GetOrUpdateListInfoAsync(key, siteService, list).PreserveThreadContext();
        }

        /// <summary>
        /// Gets the list type information asynchronously.
        /// </summary>
        /// <param name="siteId">Identifier for the site.</param>
        /// <param name="listId">Identifier for the list.</param>
        /// <param name="cancellationToken">Optional. a token that allows processing to be
        ///                                 cancelled.</param>
        /// <returns>
        /// An asynchronous result that yields the list type information.
        /// </returns>
        public async Task<IListInfo> GetListInfoAsync(Guid siteId, Guid listId, CancellationToken cancellationToken = default)
        {
            var key = new ListIdentity(siteId, listId);
            this.listInfos.TryGetValue(key, out var typeInfo);
            if (typeInfo != null)
            {
                return typeInfo;
            }

            var siteService = this.siteServiceProvider.GetSiteService(siteId);
            var list = await siteService.GetListAsync(listId).PreserveThreadContext();

            return await this.GetOrUpdateListInfoAsync(key, siteService, list).PreserveThreadContext();
        }

        private async Task<IListInfo> GetOrUpdateListInfoAsync(ListIdentity key, ISiteService siteService, List list)
        {
            var clientContext = siteService.ClientContext;
            var fields = list.Fields;
            clientContext.Load(list, l => l.Id, l => l.Title, l => l.ParentWebUrl);
            clientContext.Load(fields);
            await clientContext.ExecuteQueryAsync().PreserveThreadContext();

            key.UpdateIdentity(siteService, list);
            var typeInfo = this.listInfos.GetOrAdd(key, k => new ListInfo(list, siteService.SiteUrl));
            return typeInfo;
        }

        private class ListIdentity : IEquatable<ListIdentity>
        {
            private HashSet<string> identities = new HashSet<string>();

            public ListIdentity(string listFullName)
            {
                this.identities.Add(listFullName.ToLower());
            }

            public ListIdentity(Guid siteId, string listName)
            {
                this.identities.Add($"{siteId:N}/{listName}".ToLower());
            }

            public ListIdentity(Guid siteId, Guid listId)
            {
                this.identities.Add($"{siteId:N}/{listId:N}".ToLower());
            }

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>
            /// <see langword="true" /> if the current object is equal to the <paramref name="other" />
            /// parameter; otherwise, <see langword="false" />.
            /// </returns>
            public bool Equals(ListIdentity other)
            {
                if (other == null)
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return this.identities.Any(id => other.identities.Contains(id));
            }

            /// <summary>
            /// Updates the identity.
            /// </summary>
            /// <param name="siteService">The site service.</param>
            /// <param name="list">The list.</param>
            public void UpdateIdentity(ISiteService siteService, List list)
            {
                this.identities.Add($"{siteService.SiteName}/{list.Title}".ToLower());
                this.identities.Add($"{siteService.SiteUrl}/{list.Title}".ToLower());
                this.identities.Add($"{siteService.SiteId:N}/{list.Id:N}".ToLower());
                this.identities.Add($"{siteService.SiteId:N}/{list.Title}".ToLower());
            }
        }
    }
}
