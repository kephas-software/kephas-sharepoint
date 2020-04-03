// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISharePointUpdaterService.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the ISharePointUpdaterService interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Operations;
    using Kephas.Services;

    /// <summary>
    /// Interface for SharePoint updater service.
    /// Typically, the implementation will aggregate multiple <see cref="IListUpdaterService"/> instances,
    /// one for each site, delegating the operation to the respective service.
    /// </summary>
    [SingletonAppServiceContract]
    public interface ISharePointUpdaterService
    {
        /// <summary>
        /// Updates the list item asynchronously.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result that yields the operation result.
        /// </returns>
        Task<IOperationResult> UpdateListItemAsync(ListItem listItem, IContext context, CancellationToken cancellationToken);
    }
}
