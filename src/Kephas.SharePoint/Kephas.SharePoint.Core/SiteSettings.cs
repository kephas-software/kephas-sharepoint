// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SiteSettings.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the SharePoint site settings class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using Kephas.SharePoint.Security;

    /// <summary>
    /// A SharePoint site settings.
    /// </summary>
    public class SiteSettings
    {
        /// <summary>
        /// Gets or sets the credential used for the connection.
        /// </summary>
        /// <value>
        /// The credential.
        /// </value>
        public ISiteCredential? Credential { get; set; }

        /// <summary>
        /// Gets or sets URL of the site.
        /// </summary>
        /// <value>
        /// The site URL.
        /// </value>
        public string SiteUrl { get; set; }
    }
}
