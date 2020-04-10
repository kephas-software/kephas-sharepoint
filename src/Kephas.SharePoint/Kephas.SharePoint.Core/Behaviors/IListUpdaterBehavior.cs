// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IListUpdaterBehavior.cs" company="Kephas Software SRL">
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
    /// Behavior for the list updater service.
    /// </summary>
    [AppServiceContract]
    public interface IListUpdaterBehavior
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
        Task BeforeUpdateListItemAsync(ListItem listItem, IContext context, CancellationToken cancellationToken);

        /// <summary>
        /// Invoked after the list item is updated asynchronously.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result.
        /// </returns>
        Task AfterUpdateListItemAsync(ListItem listItem, IContext context, CancellationToken cancellationToken);
    }
}