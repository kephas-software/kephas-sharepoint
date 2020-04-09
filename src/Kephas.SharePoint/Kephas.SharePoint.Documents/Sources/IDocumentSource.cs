// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDocumentSource.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the IDocumentSource interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Sources
{
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Operations;
    using Kephas.Services;
    using Kephas.Workflow;

    /// <summary>
    /// Interface for a document source.
    /// </summary>
    [SingletonAppServiceContract(AllowMultiple = true)]
    public interface IDocumentSource : IAsyncInitializable, IAsyncFinalizable
    {
        /// <summary>
        /// Uploads the pending documents asynchronously.
        /// </summary>
        /// <param name="context">Optional. The context.</param>
        /// <param name="cancellationToken">Optional. The cancellation token.</param>
        /// <returns>
        /// An asynchronous result that yields an operation result indicating whether there is more work to do.
        /// </returns>
        Task<IOperationResult<bool>> UploadPendingDocumentsAsync(
            IContext? context = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retries to upload the failed documents asynchronously.
        /// </summary>
        /// <param name="retryContext">Context for the retry.</param>
        /// <param name="cancellationToken">Optional. A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result.
        /// </returns>
        Task<IOperationResult> RetryUploadFailedDocumentsAsync(
            IActivityContext retryContext,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Handles the provided document, preparing it for upload
        /// and eventually uploading it.
        /// </summary>
        /// <remarks>
        /// This method is typically called when redirecting from one source to another.
        /// Once a document was redirected, should not be redirected anymore.
        /// </remarks>
        /// <param name="doc">The document to handle.</param>
        /// <param name="context">Optional. The handling context.</param>
        /// <param name="cancellationToken">Optional. The cancellation token.</param>
        /// <returns>
        /// An asynchronous result that yields an operation result.
        /// </returns>
        Task<IOperationResult> HandleDocumentAsync(
            Document doc,
            IContext? context = null,
            CancellationToken cancellationToken = default);
    }
}
