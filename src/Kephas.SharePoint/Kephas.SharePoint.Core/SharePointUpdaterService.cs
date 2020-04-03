// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharePointUpdaterService.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the ISharePointUpdaterService interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Composition;
    using Kephas.Configuration;
    using Kephas.Dynamic;
    using Kephas.Logging;
    using Kephas.Operations;
    using Kephas.Services;
    using Kephas.Services.Transitions;
    using Kephas.Threading.Tasks;

    /// <summary>
    /// The default SharePoint updater service.
    /// </summary>
    [OverridePriority(Priority.Low)]
    public class SharePointUpdaterService : Loggable, ISharePointUpdaterService, IAsyncInitializable, IAsyncFinalizable
    {
        private readonly InitializationMonitor<ISharePointUpdaterService> initMonitor;
        private readonly IContextFactory contextFactory;
        private readonly IExportFactory<IListUpdaterService> siteUploaderFactory;
        private readonly IListService listService;
        private readonly IDictionary<string, IListUpdaterService> siteUploadersMap = new Dictionary<string, IListUpdaterService>();
        private SharePointSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointUpdaterService"/> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="sharepointConfiguration">The SharePoint configuration.</param>
        /// <param name="siteUploaderFactory">The site uploader factory.</param>
        /// <param name="listService">The library service.</param>
        /// <param name="logManager">Manager for log.</param>
        public SharePointUpdaterService(
            IContextFactory contextFactory,
            IConfiguration<SharePointSettings> sharepointConfiguration,
            IExportFactory<IListUpdaterService> siteUploaderFactory,
            IListService listService,
            ILogManager logManager)
            : base(logManager)
        {
            this.settings = sharepointConfiguration.Settings;
            this.contextFactory = contextFactory;
            this.siteUploaderFactory = siteUploaderFactory;
            this.listService = listService;
            this.initMonitor = new InitializationMonitor<ISharePointUpdaterService>(this.GetType());
        }

        /// <summary>
        /// Initializes the service asynchronously.
        /// </summary>
        /// <param name="context">Optional. An optional context for initialization.</param>
        /// <param name="cancellationToken">Optional. The cancellation token.</param>
        /// <returns>
        /// An awaitable task.
        /// </returns>
        public async Task InitializeAsync(IContext context = null, CancellationToken cancellationToken = default)
        {
            this.initMonitor.Start();

            var logger = this.Logger.Merge(context?.Logger);
            foreach (var siteSettingsPair in this.settings.Sites)
            {
                if (siteSettingsPair.Value == null)
                {
                    continue;
                }

                try
                {
                    var siteContext = this.contextFactory.CreateContext<Context>().Merge(context);
                    siteContext[nameof(SiteSettings)] = siteSettingsPair.Value;
                    siteContext[nameof(IListUpdaterService.SiteName)] = siteSettingsPair.Key;
                    var siteUploader = await this.siteUploaderFactory.CreateInitializedValueAsync(siteContext).PreserveThreadContext();
                    this.siteUploadersMap[siteSettingsPair.Key ?? siteSettingsPair.Value.SiteUrl] = siteUploader;
                }
                catch (Exception ex)
                {
                    this.Logger.Debug(ex, $"Could not connect to {siteSettingsPair.Value.SiteUrl}.");
                }
            }

            this.initMonitor.Complete();
        }

        /// <summary>
        /// Finalizes the service.
        /// </summary>
        /// <param name="context">Optional. An optional context for finalization.</param>
        /// <param name="cancellationToken">Optional. The cancellation token.</param>
        /// <returns>
        /// An asynchronous result.
        /// </returns>
        public async Task FinalizeAsync(IContext context = null, CancellationToken cancellationToken = default)
        {
            foreach (var siteUploader in this.siteUploadersMap.Values)
            {
                await ServiceHelper.FinalizeAsync(siteUploader, context).PreserveThreadContext();
            }

            this.initMonitor.Reset();
        }

        /// <summary>
        /// Updates the list item asynchronously.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when the requested operation is not supported.</exception>
        /// <param name="listItem">The list item.</param>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result that yields the operation result.
        /// </returns>
        public Task<IOperationResult> UpdateListItemAsync(ListItem listItem, IContext context, CancellationToken cancellationToken)
        {
            var librarySpec = this.listService.GetContainingList(listItem);
            var (siteName, _) = this.listService.GetListPathFragments(librarySpec);

            if (!this.siteUploadersMap.TryGetValue(siteName, out var siteUploader))
            {
                var message = $"SharePoint site {siteName} not configured while updating '{listItem}'.";
                this.Logger.Error(message);
                return Task.FromResult<IOperationResult>(
                    new OperationResult()
                        .MergeException(new InvalidOperationException(message))
                        .Complete(TimeSpan.Zero, OperationState.Failed));
            }

            return siteUploader.UpdateListItemAsync(listItem, context, cancellationToken);
        }
    }
}
