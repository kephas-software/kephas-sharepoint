// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SiteAccountSettings.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using Kephas.SharePoint.Security;

    /// <summary>
    /// Gets the settings for a site account.
    /// </summary>
    public class SiteAccountSettings
    {
        /// <summary>
        /// Gets or sets the credential used for the connection.
        /// </summary>
        /// <value>
        /// The credential.
        /// </value>
        public ISiteCredential? Credential { get; set; }
    }
}
