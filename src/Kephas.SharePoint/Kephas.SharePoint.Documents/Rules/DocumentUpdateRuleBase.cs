// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentUpdateRuleBase.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Rules
{
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Logging;
    using Kephas.Services;

    /// <summary>
    /// Base class for document update rules.
    /// </summary>
    public abstract class DocumentUpdateRuleBase : Loggable, IListItemUpdateRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentUpdateRuleBase"/> class.
        /// </summary>
        /// <param name="logManager">Optional. Manager for log.</param>
        public DocumentUpdateRuleBase(ILogManager? logManager = null)
            : base(logManager)
        {
        }

        /// <summary>
        /// Applies the rule asynchronously.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        /// <param name="context">Optional. The context.</param>
        /// <param name="cancellationToken">Optional. A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result.
        /// </returns>
        public Task ApplyAsync(ListItem listItem, IContext context, CancellationToken cancellationToken = default)
        {
            if (listItem is Document doc)
            {
                return this.ApplyAsync(doc, context, cancellationToken);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Applies the rule asynchronously.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="context">Optional. The context.</param>
        /// <param name="cancellationToken">Optional. A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result.
        /// </returns>
        protected abstract Task ApplyAsync(Document doc, IContext context, CancellationToken cancellationToken = default);
    }
}