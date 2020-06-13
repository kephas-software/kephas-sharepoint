// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SiteService.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the site service class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Logging;
    using Kephas.Services;
    using Kephas.Services.Transitions;
    using Kephas.Threading.Tasks;
    using Microsoft.SharePoint.Client;

    using MsListItem = Microsoft.SharePoint.Client.ListItem;

    /// <summary>
    /// A site service.
    /// </summary>
    public class SiteService : Loggable, ISiteService, IAsyncInitializable, IAsyncFinalizable
    {
        private readonly IClientContextProvider contextProvider;
        private readonly IListService libraryService;
        private readonly InitializationMonitor<ISiteService> initMonitor;
        private SiteSettings siteSettings;
        private ClientContext clientContext;
        private Timer keepAlive;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteService"/> class.
        /// </summary>
        /// <param name="contextProvider">The context provider.</param>
        /// <param name="listService">The list service.</param>
        /// <param name="logManager">Manager for log.</param>
        public SiteService(
            IClientContextProvider contextProvider,
            IListService listService,
            ILogManager logManager)
            : base(logManager)
        {
            this.contextProvider = contextProvider;
            this.libraryService = listService;
            this.initMonitor = new InitializationMonitor<ISiteService>(this.GetType());
        }

        /// <summary>
        /// Gets the identifier of the site.
        /// </summary>
        /// <value>
        /// The identifier of the site.
        /// </value>
        public Guid SiteId { get; private set; }

        /// <summary>
        /// Gets the site name.
        /// </summary>
        /// <value>
        /// The site name.
        /// </value>
        public string SiteName { get; private set; }

        /// <summary>
        /// Gets URL of the site.
        /// </summary>
        /// <value>
        /// The site URL.
        /// </value>
        public Uri SiteUrl { get; private set; }

        /// <summary>
        /// Gets a context for the client.
        /// </summary>
        /// <value>
        /// The client context.
        /// </value>
        public ClientContext ClientContext
        {
            get
            {
                this.initMonitor.AssertIsCompletedSuccessfully();

                return this.clientContext;
            }
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
            try
            {
                this.SiteName = context?[nameof(this.SiteName)] as string;
                this.siteSettings = context?[nameof(SiteSettings)] as SiteSettings;
                if (this.siteSettings == null)
                {
                    throw new SharePointException($"No site settings provided for '{this.SiteName}'.");
                }

                if (string.IsNullOrWhiteSpace(this.siteSettings.SiteUrl))
                {
                    throw new SharePointException($"No site URL provided for '{this.SiteName}'.");
                }

                this.clientContext = await this.contextProvider.GetClientContextAsync(this.siteSettings).PreserveThreadContext();

                this.SiteId = this.clientContext.Web.Id;
                this.SiteUrl = new Uri(this.siteSettings.SiteUrl);

                this.initMonitor.Complete();

                logger.Info($"Connected to SharePoint site {this.siteSettings.SiteUrl}.");

                this.keepAlive = new Timer(this.KeepAlive, context, 60 * 1000, 60 * 1000);
            }
            catch (Exception ex)
            {
                var message = $"Error while connecting to SharePoint server '{this.siteSettings.SiteUrl}'. Possible cause: bad app ID/user name and password. Check the information in the inner exception.";
                context?.Logger.Error(message);
                this.Logger.Error(ex, message);
                this.initMonitor.Reset();
            }
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
            this.keepAlive?.Dispose();
            this.clientContext?.Dispose();
        }

        /// <summary>
        /// Gets the list by name asynchronously.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="listName">Name of the list.</param>
        /// <param name="listType">Optional. Type of the list.</param>
        /// <returns>
        /// An asynchronous result that yields the list.
        /// </returns>
        public async Task<List> GetListAsync(string listName, BaseType listType = BaseType.None)
        {
            this.initMonitor.AssertIsCompletedSuccessfully();

            var list = this.GetListByName(listName);

            await this.PreloadListPropertiesAsync(list, listType).PreserveThreadContext();

            return list;
        }

        /// <summary>
        /// Gets the list by name.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="listName">Name of the list.</param>
        /// <param name="listType">Optional. Type of the list.</param>
        /// <returns>
        /// The list.
        /// </returns>
        public List GetList(string listName, BaseType listType = BaseType.None)
        {
            this.initMonitor.AssertIsCompletedSuccessfully();

            var list = this.GetListByName(listName);

            this.PreloadListProperties(list, listType);

            return list;
        }

        /// <summary>
        /// Gets the list by ID asynchronously.
        /// </summary>
        /// <param name="listId">The list identity.</param>
        /// <param name="listType">Optional. Type of the list.</param>
        /// <returns>
        /// An asynchronous result that yields the list.
        /// </returns>
        public async Task<List> GetListAsync(Guid listId, BaseType listType = BaseType.None)
        {
            this.initMonitor.AssertIsCompletedSuccessfully();

            var list = this.GetListById(listId);

            await this.PreloadListPropertiesAsync(list, listType).PreserveThreadContext();

            return list;
        }

        /// <summary>
        /// Gets the list by ID asynchronously.
        /// </summary>
        /// <param name="listId">The list identity.</param>
        /// <param name="listType">Optional. Type of the list.</param>
        /// <returns>
        /// The list.
        /// </returns>
        public List GetList(Guid listId, BaseType listType = BaseType.None)
        {
            this.initMonitor.AssertIsCompletedSuccessfully();

            var list = this.GetListById(listId);

            this.PreloadListProperties(list, listType);

            return list;
        }

        /// <summary>
        /// Gets the list items asynchronously.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="query">The query.</param>
        /// <param name="cancellationToken">Optional. A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result that yields the list items.
        /// </returns>
        public async Task<IEnumerable<MsListItem>> GetListItemsAsync(List list, CamlQuery query, CancellationToken cancellationToken = default)
        {
            this.initMonitor.AssertIsCompletedSuccessfully();

            var listItems = list.GetItems(query);
            this.clientContext.Load(listItems);
            await this.clientContext.ExecuteQueryAsync().PreserveThreadContext();

            return listItems;
        }

        /// <summary>
        /// Gets the list items.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="query">The query.</param>
        /// <returns>
        /// The list items.
        /// </returns>
        public IEnumerable<MsListItem> GetListItems(List list, CamlQuery query)
        {
            this.initMonitor.AssertIsCompletedSuccessfully();

            var listItems = list.GetItems(query);
            this.clientContext.Load(listItems);
            this.clientContext.ExecuteQuery();

            return listItems;
        }

        private List GetListById(Guid listId)
        {
            // https://code.msdn.microsoft.com/Upload-document-to-32056dbf/sourcecode?fileId=205610&pathId=1577573997

            var list = this.clientContext.Web.Lists.GetById(listId);
            if (list == null)
            {
                throw new InvalidOperationException($"List with ID '{listId} not found in {this.SiteName} ({this.SiteUrl}).");
            }

            return list;
        }

        private List GetListByName(string listName)
        {
            // https://code.msdn.microsoft.com/Upload-document-to-32056dbf/sourcecode?fileId=205610&pathId=1577573997

            listName = this.GetListName(listName);
            var list = Guid.TryParse(listName, out var listId)
                ? this.clientContext.Web.Lists.GetById(listId)
                : this.clientContext.Web.Lists.GetByTitle(listName);
            if (list == null)
            {
                throw new InvalidOperationException($"List {listName} not found in {this.SiteName} ({this.SiteUrl}).");
            }

            return list;
        }

        private string GetListName(string listName)
        {
            if (!this.libraryService.IsListFullName(listName))
            {
                return listName;
            }

            return this.libraryService.GetListPathFragments(listName).listName;
        }

        private async Task PreloadListPropertiesAsync(List list, BaseType listType)
        {
            this.PrepareListProperties(list);
            await this.clientContext.ExecuteQueryAsync().PreserveThreadContext();
            this.AssertListType(list, listType);
        }

        private void PreloadListProperties(List list, BaseType listType)
        {
            this.PrepareListProperties(list);
            this.clientContext.ExecuteQuery();
            this.AssertListType(list, listType);
        }

        private void AssertListType(List list, BaseType listType)
        {
            if (listType != BaseType.None && list.BaseType != listType)
            {
                throw new InvalidOperationException($"List '{list.Title}' must be a {listType}, instead it is a {list.BaseType}.");
            }
        }

        private void PrepareListProperties(List list)
        {
            var currentUser = this.clientContext.Web.CurrentUser;
            this.clientContext.Load(this.clientContext.Web);
            this.clientContext.Load(currentUser);
            this.clientContext.Load(list, l => l.Id, l => l.Title, l => l.EffectiveBasePermissions, l => l.BaseType, l => l.BaseTemplate);
            this.clientContext.Load(list.RootFolder, f => f.ServerRelativeUrl, f => f.Name);
        }

        private void KeepAlive(object state)
        {
            if (this.clientContext.HasPendingRequest)
            {
                return;
            }

            try
            {
                this.clientContext.Load(this.clientContext.Web, s => s.Title);
                this.clientContext.ExecuteQuery();
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex, "Error while keeping the connection alive.");
            }
        }
    }
}
