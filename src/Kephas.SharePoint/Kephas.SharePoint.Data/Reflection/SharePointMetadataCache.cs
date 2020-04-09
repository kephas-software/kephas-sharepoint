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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Collections;
    using Kephas.Logging;
    using Kephas.Services;
    using Kephas.SharePoint;
    using Kephas.Threading.Tasks;
    using Microsoft.SharePoint.Client;

    /// <summary>
    /// A SharePoint metadata cache.
    /// </summary>
    [OverridePriority(Priority.Low)]
    public class SharePointMetadataCache : Loggable, ISharePointMetadataCache
    {
        private readonly IListService listService;
        private readonly ISiteServiceProvider siteServiceProvider;
        private readonly object listInfoMapSync = new object();
        private readonly IList<(ListIdentity listIdentity, IListInfo listInfo)> listInfoMap =
            new List<(ListIdentity listIdentity, IListInfo listInfo)>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointMetadataCache"/> class.
        /// </summary>
        /// <param name="listService">The list service.</param>
        /// <param name="siteServiceProvider">The site service provider.</param>
        /// <param name="logManager">Optional. Manager for log.</param>
        public SharePointMetadataCache(IListService listService, ISiteServiceProvider siteServiceProvider, ILogManager logManager = null)
            : base(logManager)
        {
            this.listService = listService;
            this.siteServiceProvider = siteServiceProvider;
        }

        /// <summary>
        /// Clears the cache to its blank/initial state.
        /// </summary>
        public void Clear()
        {
            lock (this.listInfoMapSync)
            {
                this.listInfoMap.Clear();
            }
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
            var (key, typeInfo) = this.TryGetListEntry(new ListIdentity(listFullName));
            if (typeInfo != null)
            {
                return typeInfo;
            }

            var (siteName, listName) = this.listService.GetListPathFragments(listFullName);
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
            var (key, typeInfo) = this.TryGetListEntry(new ListIdentity(siteId, listName));
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
            var (key, typeInfo) = this.TryGetListEntry(new ListIdentity(siteId, listId));
            if (typeInfo != null)
            {
                return typeInfo;
            }

            var siteService = this.siteServiceProvider.GetSiteService(siteId);
            var list = await siteService.GetListAsync(listId).PreserveThreadContext();

            return await this.GetOrUpdateListInfoAsync(key, siteService, list).PreserveThreadContext();
        }

        /// <summary>
        /// Invalidates teh entry for the provided list.
        /// </summary>
        /// <param name="siteId">Identifier for the site.</param>
        /// <param name="listName">Name of the list.</param>
        public void Invalidate(Guid siteId, string listName)
        {
            if (!this.TryRemoveListEntry(new ListIdentity(siteId, listName)))
            {
                this.Logger.Info("List {list} not found to invalidate the cache.", $"{siteId}/{listName}");
            }
        }

        /// <summary>
        /// Invalidates teh entry for the provided list.
        /// </summary>
        /// <param name="listFullName">Full name of the list.</param>
        public void Invalidate(string listFullName)
        {
            if (!this.TryRemoveListEntry(new ListIdentity(listFullName)))
            {
                this.Logger.Info("List {list} not found to invalidate the cache.", listFullName);
            }
        }

        private (ListIdentity listIdentity, IListInfo listInfo) TryGetListEntry(ListIdentity key)
        {
            lock (this.listInfoMapSync)
            {
                var result = this.listInfoMap.FirstOrDefault(e => e.listIdentity.Equals(key));
                return result.listIdentity == null ? (key, null) : result;
            }
        }

        private bool TryRemoveListEntry(ListIdentity key)
        {
            lock (this.listInfoMapSync)
            {
                for (var i = 0; i < this.listInfoMap.Count; i++)
                {
                    if (this.listInfoMap[i].listIdentity == key)
                    {
                        this.listInfoMap.RemoveAt(i);
                        return true;
                    }
                }
            }

            return false;
        }

        private async Task<IListInfo> GetOrUpdateListInfoAsync(ListIdentity key, ISiteService siteService, List list)
        {
            var clientContext = siteService.ClientContext;
            var fields = list.Fields;
            clientContext.Load(list, l => l.Id, l => l.Title, l => l.ParentWebUrl);
            clientContext.Load(fields);
            await clientContext.ExecuteQueryAsync().PreserveThreadContext();

            key.UpdateIdentity(siteService, list);

            lock (this.listInfoMapSync)
            {
                var typeInfo = this.listInfoMap.FirstOrDefault(e => e.listIdentity.Equals(key)).listInfo;
                if (typeInfo != null)
                {
                    return typeInfo;
                }

                typeInfo = new ListInfo(list, siteService.SiteName ?? siteService.SiteUrl.ToString(), siteService.SiteUrl.ToString());
                this.listInfoMap.Add((key, typeInfo));
                return typeInfo;
            }
        }

        internal class ListIdentity : IEquatable<ListIdentity>
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
            /// Determines whether the specified object is equal to the current object.
            /// </summary>
            /// <param name="obj">The object to compare with the current object.</param>
            /// <returns>
            /// <see langword="true" /> if the specified object  is equal to the current object; otherwise,
            /// <see langword="false" />.
            /// </returns>
            public override bool Equals(object obj) => this.Equals(obj as ListIdentity);

            /// <summary>
            /// Updates the identity.
            /// </summary>
            /// <param name="siteService">The site service.</param>
            /// <param name="list">The list.</param>
            public void UpdateIdentity(ISiteService siteService, List list)
            {
                this.identities.Add($"{siteService.SiteName}/{list.Title}".ToLower());
                this.identities.Add($"{siteService.SiteUrl}/{list.Title}".ToLower());
                this.identities.Add($"{siteService.SiteUrl:N}/{list.Id:N}".ToLower());
                this.identities.Add($"{siteService.SiteId:N}/{list.Title}".ToLower());
                this.identities.Add($"{siteService.SiteId:N}/{list.Id:N}".ToLower());
            }

            /// <summary>
            /// Updates the identity.
            /// </summary>
            /// <param name="identities">The identities.</param>
            internal void UpdateIdentity(IEnumerable<string> identities)
            {
                this.identities.AddRange(identities);
            }
        }
    }
}
