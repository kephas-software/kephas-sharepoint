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
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Services;

    /// <summary>
    /// Interface for SharePoint metadata cache.
    /// </summary>
    [SingletonAppServiceContract]
    public interface ISharePointMetadataCache
    {
        /// <summary>
        /// Gets the list type information asynchronously.
        /// </summary>
        /// <param name="listFullName">Full name of the list.</param>
        /// <param name="cancellationToken">Optional. a token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result that yields the list type information.
        /// </returns>
        Task<IListTypeInfo> GetListTypeInfoAsync(string listFullName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Clears the cache to its blank/initial state.
        /// </summary>
        void Clear();
    }
}
