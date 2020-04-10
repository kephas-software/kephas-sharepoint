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
    using Kephas.Threading.Tasks;

    /// <summary>
    /// Base class for list updaters.
    /// </summary>
    public abstract class ListUpdaterServiceBase : Loggable, IListUpdaterService
    {
        private readonly ICollection<Lazy<IListUpdaterBehavior, AppServiceMetadata>> listItemUpdaters;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListUpdaterServiceBase"/> class.
        /// </summary>
        /// <param name="listItemUpdaters">The list item updaters.</param>
        /// <param name="logManager">Optional. The log manager.</param>
        protected ListUpdaterServiceBase(
            ICollection<Lazy<IListUpdaterBehavior, AppServiceMetadata>> listItemUpdaters,
            ILogManager? logManager = null)
            : base(logManager)
        {
            this.listItemUpdaters = listItemUpdaters.Order().ToList();
        }

        /// <summary>
        /// Gets or sets the site name.
        /// </summary>
        /// <value>
        /// The site name.
        /// </value>
        public string SiteName { get; protected set; }

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
            IOperationResult coreResult = null;
            var opResult = await Profiler.WithStopwatchAsync(
                async () =>
                {
                    await this.ApplyBeforeBehaviorsAsync(listItem, context, cancellationToken).PreserveThreadContext();

                    coreResult = await this.UpdateListItemCoreAsync(listItem, context, cancellationToken)
                        .PreserveThreadContext();

                    await this.ApplyAfterBehaviorsAsync(listItem, context, cancellationToken).PreserveThreadContext();
                }).PreserveThreadContext();

            return opResult
                .MergeMessages(coreResult)
                .ReturnValue(coreResult.ReturnValue);
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