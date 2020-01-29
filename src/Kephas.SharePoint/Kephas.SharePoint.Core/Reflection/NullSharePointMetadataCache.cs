// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullSharePointMetadataCache.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the null SharePoint metadata cache class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Reflection
{
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Services;

    /// <summary>
    /// A null SharePoint metadata cache.
    /// </summary>
    [OverridePriority(Priority.Lowest)]
    public class NullSharePointMetadataCache : ISharePointMetadataCache
    {
        /// <summary>
        /// Clears this object to its blank/initial state.
        /// </summary>
        public void Clear()
        {
        }

        /// <summary>
        /// Gets the list type information asynchronously.
        /// </summary>
        /// <param name="listFullName">Full name of the list.</param>
        /// <param name="cancellationToken">Optional. a token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result that yields the list type information.
        /// </returns>
        public Task<IListInfo> GetListTypeInfoAsync(string listFullName, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IListInfo>(null);
        }
    }
}
