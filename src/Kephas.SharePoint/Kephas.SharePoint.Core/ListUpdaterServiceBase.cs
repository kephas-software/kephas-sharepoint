// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListUpdaterServiceBase.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Diagnostics;
    using Kephas.Logging;
    using Kephas.Operations;
    using Kephas.Services;
    using Kephas.Services.Composition;
    using Kephas.SharePoint.Behaviors;
    using Kephas.Threading.Tasks;

    /// <summary>
    /// Base class for list updaters.
    /// </summary>
    public abstract class ListUpdaterServiceBase : Loggable, IListUpdaterService, IAsyncInitializable, IAsyncFinalizable
    {
        private readonly ICollection<Lazy<IListUpdaterBehavior, AppServiceMetadata>> listItemUpdaters;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListUpdaterServiceBase"/> class.
        /// </summary>
        /// <param name="listItemUpdaterBehaviors">The list item updater behaviors.</param>
        /// <param name="logManager">Optional. The log manager.</param>
        protected ListUpdaterServiceBase(
            ICollection<Lazy<IListUpdaterBehavior, AppServiceMetadata>> listItemUpdaterBehaviors,
            ILogManager? logManager = null)
            : base(logManager)
        {
            this.listItemUpdaters = listItemUpdaterBehaviors.Order().ToList();
        }

        /// <summary>
        /// Gets the site name.
        /// </summary>
        /// <value>
        /// The site name.
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
        public virtual Task InitializeAsync(IContext? context = null, CancellationToken cancellationToken = default)
        {
            this.SiteName = context?[nameof(IListUpdaterService.SiteName)] as string ?? string.Empty;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Finalizes the service asynchronously.
        /// </summary>
        /// <param name="context">Optional. An optional context for finalization.</param>
        /// <param name="cancellationToken">Optional. The cancellation token.</param>
        /// <returns>
        /// An asynchronous result.
        /// </returns>
        public virtual Task FinalizeAsync(IContext? context = null, CancellationToken cancellationToken = default)
        {
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
        public async Task<IOperationResult> UpdateListItemAsync(ListItem listItem, IContext context, CancellationToken cancellationToken)
        {
            return (await Profiler.WithStopwatchAsync(
                async () =>
                {
                    await this.ApplyBeforeBehaviorsAsync(listItem, context, cancellationToken).PreserveThreadContext();

                    var result = await this.UpdateListItemCoreAsync(listItem, context, cancellationToken)
                        .PreserveThreadContext();

                    await this.ApplyAfterBehaviorsAsync(listItem, context, cancellationToken).PreserveThreadContext();
                    return result;
                }).PreserveThreadContext())
                .Flatten();
        }

        /// <summary>
        /// Updates the list item asynchronously (core implementation).
        /// </summary>
        /// <param name="listItem">The list item.</param>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result that yields the operation result.
        /// </returns>
        protected abstract Task<IOperationResult> UpdateListItemCoreAsync(
            ListItem listItem,
            IContext context,
            CancellationToken cancellationToken);

        /// <summary>
        /// Applies the behaviors before updating the list item.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// An asynchronous result.
        /// </returns>
        protected virtual async Task ApplyBeforeBehaviorsAsync(ListItem listItem, IContext context, CancellationToken cancellationToken)
        {
            foreach (var lu in this.listItemUpdaters)
            {
                await lu.Value.BeforeUpdateListItemAsync(listItem, context, cancellationToken).PreserveThreadContext();
            }
        }

        /// <summary>
        /// Applies the behaviors before the list item has been updated.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// An asynchronous result.
        /// </returns>
        protected virtual async Task ApplyAfterBehaviorsAsync(ListItem listItem, IContext context, CancellationToken cancellationToken)
        {
            foreach (var lu in this.listItemUpdaters.Reverse())
            {
                await lu.Value.AfterUpdateListItemAsync(listItem, context, cancellationToken).PreserveThreadContext();
            }
        }
    }
}