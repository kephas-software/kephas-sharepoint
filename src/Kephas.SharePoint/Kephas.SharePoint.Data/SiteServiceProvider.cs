// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SiteServiceProvider.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the site service provider class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Collections;
    using Kephas.Composition;
    using Kephas.Dynamic;
    using Kephas.Logging;
    using Kephas.Services;
    using Kephas.Services.Transitions;
    using Kephas.Threading.Tasks;

    /// <summary>
    /// A site service provider.
    /// </summary>
    public class SiteServiceProvider : Loggable, ISiteServiceProvider, IAsyncInitializable, IAsyncFinalizable
    {
        private readonly InitializationMonitor<ISiteServiceProvider> initMonitor;
        private readonly IDictionary<string, ISiteService> siteServicesMap = new Dictionary<string, ISiteService>();
        private readonly IContextFactory contextFactory;
        private readonly ISiteSettingsProvider siteSettingsProvider;
        private readonly IExportFactory<ISiteService> siteServiceFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteServiceProvider"/> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="siteSettingsProvider">The site settings provider.</param>
        /// <param name="siteServiceFactory">The site service factory.</param>
        /// <param name="logManager">Manager for log.</param>
        public SiteServiceProvider(
            IContextFactory contextFactory,
            ISiteSettingsProvider siteSettingsProvider,
            IExportFactory<ISiteService> siteServiceFactory,
            ILogManager logManager)
            : base(logManager)
        {
            this.contextFactory = contextFactory;
            this.siteSettingsProvider = siteSettingsProvider;
            this.siteServiceFactory = siteServiceFactory;
            this.initMonitor = new InitializationMonitor<ISiteServiceProvider>(this.GetType());
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
            foreach (var (siteName, siteSettings) in this.siteSettingsProvider.GetSiteSettings())
            {
                if (siteName == null)
                {
                    continue;
                }

                try
                {
                    var siteContext = this.contextFactory.CreateContext<Context>().Merge(context);
                    siteContext[nameof(SiteSettings)] = siteSettings;
                    siteContext[nameof(ISiteService.SiteName)] = siteName;
                    var siteService = await this.siteServiceFactory.CreateInitializedValueAsync(siteContext).PreserveThreadContext();
                    this.siteServicesMap[siteName] = siteService;
                }
                catch (Exception ex)
                {
                    this.Logger.Debug(ex, $"Could not connect to {siteSettings.SiteUrl}.");
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
            foreach (var siteService in this.siteServicesMap.Values)
            {
                await ServiceHelper.FinalizeAsync(siteService, context, cancellationToken).PreserveThreadContext();
            }

            this.initMonitor.Reset();
        }

        /// <summary>
        /// Gets the site service for the provided site name.
        /// </summary>
        /// <param name="siteName">Name of the site.</param>
        /// <param name="throwOnNotFound">Optional. True to throw on not found.</param>
        /// <returns>
        /// The site service.
        /// </returns>
        public ISiteService GetSiteService(string siteName, bool throwOnNotFound = true)
        {
            this.initMonitor.AssertIsCompletedSuccessfully();

            var siteService = Guid.TryParse(siteName, out var siteId)
                ? this.siteServicesMap.FirstOrDefault(svc => svc.Value.SiteId == siteId).Value
                : this.siteServicesMap.TryGetValue(siteName);

            if (siteService == null)
            {
                if (throwOnNotFound)
                {
                    throw new KeyNotFoundException($"SharePoint site {siteName} not configured.");
                }
            }

            return siteService;
        }

        /// <summary>
        /// Gets the site service for the provided site name.
        /// </summary>
        /// <param name="siteId">Name of the site.</param>
        /// <param name="throwOnNotFound">Optional. True to throw on not found.</param>
        /// <returns>
        /// The site service.
        /// </returns>
        public ISiteService GetSiteService(Guid siteId, bool throwOnNotFound = true)
        {
            this.initMonitor.AssertIsCompletedSuccessfully();

            var siteService = this.siteServicesMap.FirstOrDefault(svc => svc.Value.SiteId == siteId).Value;
            if (siteService == null)
            {
                if (throwOnNotFound)
                {
                    throw new KeyNotFoundException($"SharePoint site with ID '{siteId}' not found. Possible resolution: There may be cases when lookup fields reference lists from other sites; please configure these sites too.");
                }
            }

            return siteService;
        }

        /// <summary>
        /// Gets the default site service.
        /// </summary>
        /// <exception cref="KeyNotFoundException">Thrown when a Key Not Found error condition occurs.</exception>
        /// <param name="throwOnNotFound">Optional. True to throw on not found.</param>
        /// <returns>
        /// The default site service.
        /// </returns>
        public ISiteService GetDefaultSiteService(bool throwOnNotFound = true)
        {
            this.initMonitor.AssertIsCompletedSuccessfully();

            if (this.siteServicesMap.Count == 1)
            {
                return this.siteServicesMap.First().Value;
            }

            if (this.siteServicesMap.Count > 1)
            {
                throw new KeyNotFoundException($"No default SharePoint site is configured, multiple provided.");
            }

            if (throwOnNotFound)
            {
                throw new KeyNotFoundException($"No SharePoint sites are configured.");
            }

            return null;
        }
    }
}
