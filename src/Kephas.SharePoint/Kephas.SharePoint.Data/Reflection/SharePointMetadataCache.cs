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

    using Kephas.Reflection;
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
        private readonly ILibraryService libraryService;
        private readonly ISiteServiceProvider siteServiceProvider;
        private readonly ConcurrentDictionary<ListIdentity, ITypeInfo> listTypeInfos = new ConcurrentDictionary<ListIdentity, ITypeInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointMetadataCache"/> class.
        /// </summary>
        /// <param name="libraryService">The library service.</param>
        /// <param name="siteServiceProvider">The site service provider.</param>
        public SharePointMetadataCache(ILibraryService libraryService, ISiteServiceProvider siteServiceProvider)
        {
            this.libraryService = libraryService;
            this.siteServiceProvider = siteServiceProvider;
        }

        /// <summary>
        /// Clears the cache to its blank/initial state.
        /// </summary>
        public void Clear()
        {
            this.listTypeInfos.Clear();
        }

        /// <summary>
        /// Gets the list type information asynchronously.
        /// </summary>
        /// <param name="listFullName">Full name of the list.</param>
        /// <param name="cancellationToken">Optional. a token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result that yields the list type information.
        /// </returns>
        public async Task<ITypeInfo> GetListTypeInfoAsync(string listFullName, CancellationToken cancellationToken = default)
        {
            var key = new ListIdentity(listFullName);
            this.listTypeInfos.TryGetValue(key, out var typeInfo);
            if (typeInfo != null)
            {
                return typeInfo;
            }

            var (siteName, listName) = this.libraryService.GetLibraryPathFragments(listFullName);
            var siteService = this.siteServiceProvider.GetSiteService(siteName);
            var list = await siteService.GetListAsync(listFullName).PreserveThreadContext();

            var clientContext = siteService.ClientContext;
            var fields = list.Fields;
            clientContext.Load(list, l => l.Title, l => l.ParentWebUrl);
            clientContext.Load(fields);
            await clientContext.ExecuteQueryAsync().PreserveThreadContext();

            key.UpdateIdentity(siteService, list);
            typeInfo = this.listTypeInfos.GetOrAdd(key, k => new ListTypeInfo(list, siteService.SiteUrl));
            return typeInfo;
        }

        private class ListIdentity : IEquatable<ListIdentity>
        {
            private HashSet<string> identities = new HashSet<string>();

            public ListIdentity(string listFullName)
            {
                this.identities.Add(listFullName.ToLower());
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
            }
        }
    }
}
