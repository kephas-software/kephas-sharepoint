// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharePointDataContextSettings.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the SharePoint data context settings class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Data
{
    using Kephas.Data;
    using Kephas.SharePoint;

    /// <summary>
    /// A SharePoint data context settings.
    /// </summary>
    public class SharePointDataContextSettings : DataContextSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointDataContextSettings"/> class.
        /// </summary>
        /// <param name="siteSettingsProvider">The site settings provider.</param>
        public SharePointDataContextSettings(ISiteSettingsProvider siteSettingsProvider)
            : base(string.Empty)
        {
            this.SiteSettingsProvider = siteSettingsProvider;
        }

        /// <summary>
        /// Gets the site settings provider.
        /// </summary>
        /// <value>
        /// The site settings provider.
        /// </value>
        public ISiteSettingsProvider SiteSettingsProvider { get; }
    }
}
