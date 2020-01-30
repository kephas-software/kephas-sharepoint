// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITextFinder.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the ITextFinder interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.TextProcessing
{
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Services;

    /// <summary>
    /// Interface for text finder.
    /// </summary>
    [AppServiceContract(AllowMultiple = true)]
    public interface ITextFinder
    {
        /// <summary>
        /// Searches for the text matching the query in the document asynchronously.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="doc">The document.</param>
        /// <param name="context">The find context.</param>
        /// <param name="cancellationToken">Optional. A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result that yields the find result.
        /// </returns>
        Task<IFindResult> FindAsync(string query, Document doc, IFindContext context, CancellationToken cancellationToken = default);
    }
}
