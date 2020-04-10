// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IListItemUpdateRule.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Rules
{
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Services;

    /// <summary>
    /// A rule for updating the list item.
    /// </summary>
    [AppServiceContract(AllowMultiple = true)]
    public interface IListItemUpdateRule
    {
        /// <summary>
        /// Applies the rule asynchronously.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">Optional. A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result.
        /// </returns>
        Task ApplyAsync(ListItem listItem, IContext context, CancellationToken cancellationToken = default);
    }
}