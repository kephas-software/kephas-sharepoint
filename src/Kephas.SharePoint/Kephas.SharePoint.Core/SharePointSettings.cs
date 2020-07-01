// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharePointSettings.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the SharePoint settings class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using System.Collections.Generic;

    using Kephas.SharePoint.Configuration;

    /// <summary>
    /// The SharePoint settings.
    /// </summary>
    public class SharePointSettings
    {
        /// <summary>
        /// Gets or sets the sites.
        /// </summary>
        /// <value>
        /// The sites.
        /// </value>
        public IDictionary<string, SiteAccountSettings> Accounts { get; set; } = new Dictionary<string, SiteAccountSettings>();

        /// <summary>
        /// Gets or sets the sites.
        /// </summary>
        /// <value>
        /// The sites.
        /// </value>
        public IDictionary<string, SiteSettings> Sites { get; set; } = new Dictionary<string, SiteSettings>();

        /// <summary>
        /// Gets or sets the defaults.
        /// </summary>
        /// <value>
        /// The defaults.
        /// </value>
        public DefaultSettings Defaults { get; set; } = new DefaultSettings();
    }
}
