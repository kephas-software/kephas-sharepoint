// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILookupService.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the ILookupListCache interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Operations;
    using Kephas.Services;
    using Kephas.SharePoint.TextProcessing;

    /// <summary>
    /// Interface for lookup service.
    /// </summary>
    [SingletonAppServiceContract]
    public interface ILookupService
    {
        /// <summary>
        /// Tries to find the reference asynchronously.
        /// </summary>
        /// <param name="siteId">Identifier for the site.</param>
        /// <param name="listId">Identifier for the list.</param>
        /// <param name="refField">The reference field.</param>
        /// <param name="doc">The document to be searched for.</param>
        /// <param name="textFinder">The text finder.</param>
        /// <param name="cancellationToken">Optional. A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result that yields the reference values and a success flag.
        /// </returns>
        Task<IOperationResult<(int? id, object value, bool success)>> TryFindReferenceAsync(Guid siteId, string listId, string refField, Document doc, ITextFinder textFinder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tries to find the reference asynchronously.
        /// </summary>
        /// <param name="siteId">Identifier for the site.</param>
        /// <param name="listId">Identifier for the list.</param>
        /// <param name="refField">The reference field.</param>
        /// <param name="refValue">The reference value.</param>
        /// <param name="cancellationToken">Optional. A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result that yields the reference values and a success flag.
        /// </returns>
        Task<IOperationResult<(int? id, object value, bool success)>> TryFindReferenceAsync(Guid siteId, string listId, string refField, object refValue, CancellationToken cancellationToken = default);
    }
}
