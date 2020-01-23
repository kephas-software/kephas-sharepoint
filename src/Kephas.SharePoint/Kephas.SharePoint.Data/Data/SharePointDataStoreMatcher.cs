// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharePointDataStoreMatcher.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the SharePoint data store matcher class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Kephas.SharePoint.Data
{
    using System;

    using Kephas.Data.Store;
    using Kephas.Services;
    using Kephas.SharePoint;
    using Kephas.SharePoint.Activation;

    /// <summary>
    /// A SharePoint data store matcher.
    /// </summary>
    [ProcessingPriority(Priority.Low)]
    public class SharePointDataStoreMatcher : IDataStoreMatcher
    {
        private readonly ISiteSettingsProvider siteSettingsProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointDataStoreMatcher"/> class.
        /// </summary>
        /// <param name="siteSettingsProvider">The site settings provider.</param>
        public SharePointDataStoreMatcher(ISiteSettingsProvider siteSettingsProvider)
        {
            this.siteSettingsProvider = siteSettingsProvider;
        }

        /// <summary>
        /// Gets the data store.
        /// </summary>
        /// <param name="dataStoreName">Name of the data store.</param>
        /// <param name="context">Optional. The context.</param>
        /// <returns>
        /// The data store.
        /// </returns>
        public (IDataStore dataStore, bool canHandle) GetDataStore(string dataStoreName, IContext context = null)
        {
            if (dataStoreName == SharePointDataContext.DataStoreKind)
            {
                return (new DataStore(
                                SharePointDataContext.DataStoreKind,
                                SharePointDataContext.DataStoreKind,
                                typeof(SharePointDataContext),
                                new SharePointDataContextSettings(this.siteSettingsProvider),
                                new SharePointEntityActivator()),
                        true);
            }

            return (null, false);
        }

        /// <summary>
        /// Gets the data store name for the provided entity type.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="context">Optional. The context.</param>
        /// <returns>
        /// The data store name and a flag indicating whether the matching was successful.
        /// </returns>
        public (string dataStoreName, bool canHandle) GetDataStoreName(Type entityType, IContext context = null)
        {
            return entityType == typeof(ISharePointEntity) || entityType == typeof(SharePointEntity)
                ? (SharePointDataContext.DataStoreKind, true)
                : (null, false);
        }
    }
}
