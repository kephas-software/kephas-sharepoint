// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentSourceBase.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the document source base class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Sources
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Configuration;
    using Kephas.ExceptionHandling;
    using Kephas.Logging;
    using Kephas.Messaging;
    using Kephas.Messaging.Messages;
    using Kephas.Services;
    using Kephas.SharePoint;
    using Kephas.SharePoint.Configuration;
    using Kephas.Threading.Tasks;

    /// <summary>
    /// A document source base.
    /// </summary>
    /// <typeparam name="TSettings">Type of the settings.</typeparam>
    public abstract class DocumentSourceBase<TSettings> : Loggable, IDocumentSource
        where TSettings : class, new()
    {
        private readonly IMessageProcessor messageProcessor;
        private Task iterationTask;
        private bool stopIteration = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentSourceBase{TSettings}"/> class.
        /// </summary>
        /// <param name="messageProcessor">The message processor.</param>
        /// <param name="configuration">The source configuration.</param>
        /// <param name="defaultsProvider">The defaults provider.</param>
        public DocumentSourceBase(
            IMessageProcessor messageProcessor,
            IConfiguration<TSettings> configuration,
            IDefaultSettingsProvider defaultsProvider)
        {
            this.Settings = configuration.Settings;
            this.Defaults = defaultsProvider.Defaults;
            this.messageProcessor = messageProcessor;
        }

        /// <summary>
        /// Gets options for controlling the operation.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public TSettings Settings { get; }

        /// <summary>
        /// Gets the defaults.
        /// </summary>
        /// <value>
        /// The defaults.
        /// </value>
        public DefaultSettings Defaults { get; }

        /// <summary>
        /// Gets a context for the application.
        /// </summary>
        /// <value>
        /// The application context.
        /// </value>
        public IContext AppContext { get; private set; }

        /// <summary>
        /// Initializes the source asynchronously.
        /// </summary>
        /// <param name="context">Optional. The context.</param>
        /// <param name="cancellationToken">Optional. A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result.
        /// </returns>
        public virtual Task InitializeAsync(IContext context = null, CancellationToken cancellationToken = default)
        {
            this.AppContext = context;
            this.iterationTask = this.StartIterationAsync();
            return TaskHelper.CompletedTask;
        }

        /// <summary>
        /// Finalize the source asynchronously.
        /// </summary>
        /// <param name="context">Optional. The context.</param>
        /// <param name="cancellationToken">Optional. A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result.
        /// </returns>
        public virtual async Task FinalizeAsync(IContext context = null, CancellationToken cancellationToken = default)
        {
            this.stopIteration = true;
            if (this.iterationTask != null)
            {
                await this.iterationTask.PreserveThreadContext();
            }
        }

        /// <summary>
        /// Starts an iteration asynchronously.
        /// </summary>
        /// <returns>
        /// An asynchronous result.
        /// </returns>
        protected virtual async Task StartIterationAsync()
        {
            var workToDo = await this.UploadPendingDocumentsAsync().PreserveThreadContext();

            if (!this.stopIteration)
            {
                // wait 3 seconds for the next iteration;
                if (!workToDo)
                {
                    await Task.Delay(3000).PreserveThreadContext();
                }

                // start a new iteration if not required to stop.
                this.iterationTask = this.stopIteration ? null : this.StartIterationAsync();
            }
            else
            {
                this.iterationTask = null;
            }
        }

        /// <summary>
        /// Uploads the pending documents asynchronously.
        /// </summary>
        /// <returns>
        /// An asynchronous result that yields whether there is more work to do.
        /// </returns>
        protected abstract Task<bool> UploadPendingDocumentsAsync();

        /// <summary>
        /// Uploads a document asynchronous.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result that yields the upload document.
        /// </returns>
        protected virtual async Task<ResponseMessage> UploadDocumentAsync(Document doc, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var context = new MessagingContext(this.AppContext, this.messageProcessor))
                {
                    var response = (ResponseMessage)await this.messageProcessor.ProcessAsync(doc, context, cancellationToken).PreserveThreadContext();
                    return response;
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex, $"Error while uploading document '{doc}'");
                return new ResponseMessage { Message = ex.Message, Severity = SeverityLevel.Error };
            }
        }

        /// <summary>
        /// Gets the file names for the target and archive.
        /// </summary>
        /// <param name="filePath">Full pathname of the file.</param>
        /// <param name="docId">Identifier for the document.</param>
        /// <param name="preserveOriginalName">True to preserve original name.</param>
        /// <returns>
        /// The file names.
        /// </returns>
        protected virtual (string targetFileName, string archiveFileName) GetFileNames(string filePath, long docId, bool preserveOriginalName)
        {
            var fileName = Path.GetFileName(filePath);

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            var fileExtension = fileName.Substring(fileNameWithoutExtension.Length);
            var archiveFileName = $"{fileNameWithoutExtension}.{docId:x}{fileExtension}";

            return (preserveOriginalName ? fileName : archiveFileName, archiveFileName);
        }
    }
}
