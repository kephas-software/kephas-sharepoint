// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IListUpdaterService.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the IListUpdaterService interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Operations;
    using Kephas.Services;

    /// <summary>
    /// Interface for list updater service.
    /// </summary>
    [AppServiceContract]
    public interface IListUpdaterService
    {
        /// <summary>
        /// Gets the name of the site.
        /// </summary>
        /// <value>
        /// The name of the site.
        /// </value>
        public string SiteName { get; }

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
