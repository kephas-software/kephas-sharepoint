// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDocumentRule.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the IDocumentRule interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Rules
{
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Services;

    /// <summary>
    /// Interface for document rule.
    /// </summary>
    [AppServiceContract(AllowMultiple = true)]
    public interface IDocumentRule
    {
        /// <summary>
        /// Applies the rule asynchronously.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="context">Optional. The context.</param>
        /// <param name="cancellationToken">Optional. A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result.
        /// </returns>
        Task ApplyAsync(Document doc, IContext context = null, CancellationToken cancellationToken = default);
    }
}
