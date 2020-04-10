// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullListUpdaterService.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the null list updater service class.
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
    /// A null list updater service.
    /// </summary>
    [OverridePriority(Priority.Lowest)]
    public class NullListUpdaterService : IListUpdaterService, IAsyncInitializable
    {
        /// <summary>
        /// Gets the name of the site.
        /// </summary>
        /// <value>
        /// The name of the site.
        /// </value>
        public string SiteName { get; private set; } = string.Empty;

        /// <summary>
        /// Initializes the service asynchronously.
        /// </summary>
        /// <param name="context">Optional. An optional context for initialization.</param>
        /// <param name="cancellationToken">Optional. The cancellation token.</param>
        /// <returns>
        /// An awaitable task.
        /// </returns>
        public Task InitializeAsync(IContext? context = null, CancellationToken cancellationToken = default)
        {
            this.SiteName = context?[nameof(IListUpdaterService.SiteName)] as string ?? string.Empty;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Updates the list item asynchronously.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result that yields the operation result.
        /// </returns>
        public Task<IOperationResult> UpdateListItemAsync(ListItem listItem, IContext context, CancellationToken cancellationToken)
        {
                return Task.FromResult<IOperationResult>(
                    new OperationResult()
                        .MergeException(new InvalidOperationException($"Please provide a proper implementation for the {nameof(IListUpdaterService)}."))
                        .Complete(TimeSpan.Zero, OperationState.Failed));
        }
    }
}
