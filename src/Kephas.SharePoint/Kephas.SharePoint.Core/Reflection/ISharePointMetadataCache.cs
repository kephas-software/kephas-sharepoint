// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISharePointMetadataCache.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the ISharePointMetadataCache interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Reflection
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Reflection;
    using Kephas.Services;

    /// <summary>
    /// Interface for SharePoint metadata cache.
    /// </summary>
    [SingletonAppServiceContract]
    public interface ISharePointMetadataCache : ITypeRegistry
    {
        /// <summary>
        /// Gets the list type information asynchronously.
        /// </summary>
        /// <param name="listFullName">Full name of the list.</param>
        /// <param name="cancellationToken">Optional. a token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result that yields the list type information.
        /// </returns>
        Task<IListInfo> GetListInfoAsync(string listFullName, CancellationToken cancellationToken = default);

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
        Task<IListInfo> GetListInfoAsync(Guid siteId, string listName, CancellationToken cancellationToken = default);

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
        Task<IListInfo> GetListInfoAsync(Guid siteId, Guid listId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Invalidates teh entry for the provided list.
        /// </summary>
        /// <param name="listFullName">Full name of the list.</param>
        void Invalidate(string listFullName);

        /// <summary>
        /// Invalidates teh entry for the provided list.
        /// </summary>
        /// <param name="siteId">Identifier for the site.</param>
        /// <param name="listName">Name of the list.</param>
        void Invalidate(Guid siteId, string listName);

        /// <summary>
        /// Clears the cache to its blank/initial state.
        /// </summary>
        void Clear();
    }
}
