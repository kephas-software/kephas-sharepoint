// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListUpdaterBehaviorBase.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Behaviors
{
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Services;

    /// <summary>
    /// Base class for list updater behaviors.
    /// </summary>
    public abstract class ListUpdaterBehaviorBase : IListUpdaterBehavior
    {
        /// <summary>
        /// Invoked before the list item is updated asynchronously.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result.
        /// </returns>
        public virtual Task BeforeUpdateListItemAsync(ListItem listItem, IContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Invoked after the list item is updated asynchronously.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result.
        /// </returns>
        public virtual Task AfterUpdateListItemAsync(ListItem listItem, IContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}