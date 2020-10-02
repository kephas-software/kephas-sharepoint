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
        private readonly IExportFactory<IListUpdaterService> listUpdaterService;
        private readonly IListService listService;
        private readonly ISiteSettingsProvider siteSettingsProvider;
        private readonly IDictionary<string, IListUpdaterService> listUpdatersMap = new Dictionary<string, IListUpdaterService>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointUpdaterService"/> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="listUpdaterService">The list updater service.</param>
        /// <param name="listService">The library service.</param>
        /// <param name="siteSettingsProvider">The site settings provider.</param>
        /// <param name="logManager">Manager for log.</param>
        public SharePointUpdaterService(
            IContextFactory contextFactory,
            IExportFactory<IListUpdaterService> listUpdaterService,
            IListService listService,
            ISiteSettingsProvider siteSettingsProvider,
            ILogManager logManager)
            : base(logManager)
        {
            this.contextFactory = contextFactory;
            this.listUpdaterService = listUpdaterService;
            this.listService = listService;
            this.siteSettingsProvider = siteSettingsProvider;
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
        public async Task InitializeAsync(IContext? context = null, CancellationToken cancellationToken = default)
        {
            this.initMonitor.Start();

            var logger = this.Logger.Merge(context?.Logger);
            foreach (var (siteName, siteSettings) in this.siteSettingsProvider.GetSiteSettings())
            {
                if (siteSettings == null)
                {
                    continue;
                }

                try
                {
                    var siteContext = this.contextFactory.CreateContext<Context>().Merge(context);

                    var siteAccountSettings = this.siteSettingsProvider.GetSiteAccountSettings(siteName, siteSettings.Account);
                    if (siteAccountSettings == null)
                    {
                        if (string.IsNullOrEmpty(siteSettings.Account))
                        {
                            logger.Error("No account configured for site '{site}'.", siteName);
                        }
                        else
                        {
                            logger.Error("Could not find the account settings for '{account}' when connecting to site '{site}'.", siteSettings.Account, siteName);
                        }
                    }

                    siteContext[nameof(IListUpdaterService.SiteName)] = siteName;
                    siteContext[nameof(SiteSettings)] = siteSettings;
                    siteContext[nameof(SiteAccountSettings)] = siteAccountSettings;
                    var listUpdater = await this.listUpdaterService.CreateInitializedValueAsync(siteContext, cancellationToken: cancellationToken).PreserveThreadContext();
                    this.listUpdatersMap[siteName ?? siteSettings.SiteUrl] = listUpdater;
                }
                catch (Exception ex)
                {
                    logger.Warn(ex, $"Could not connect to {siteSettings.SiteUrl}.");
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
        public async Task FinalizeAsync(IContext? context = null, CancellationToken cancellationToken = default)
        {
            foreach (var siteUploader in this.listUpdatersMap.Values)
            {
                await ServiceHelper.FinalizeAsync(siteUploader, context, cancellationToken).PreserveThreadContext();
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

            if (!this.listUpdatersMap.TryGetValue(siteName, out var siteUploader))
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
