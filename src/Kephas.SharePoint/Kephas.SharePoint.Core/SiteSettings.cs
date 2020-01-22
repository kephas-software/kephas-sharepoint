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
    /// <summary>
    /// A SharePoint site settings.
    /// </summary>
    public class SiteSettings
    {
        /// <summary>
        /// Gets or sets the identifier of the application.
        /// </summary>
        /// <value>
        /// The identifier of the application.
        /// </value>
        public string AppId { get; set; }

        /// <summary>
        /// Gets or sets the application password.
        /// </summary>
        /// <value>
        /// The application password.
        /// </value>
        public string AppPassword { get; set; }

        /// <summary>
        /// Gets or sets the application encrypted password.
        /// </summary>
        /// <value>
        /// The application encrypted password.
        /// </value>
        public string AppEncryptedPassword { get; set; }

        /// <summary>
        /// Gets or sets URL of the site.
        /// </summary>
        /// <value>
        /// The site URL.
        /// </value>
        public string SiteUrl { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string UserPassword { get; set; }

        /// <summary>
        /// Gets or sets the encrypted password.
        /// </summary>
        /// <value>
        /// The encrypted password.
        /// </value>
        public string UserEncryptedPassword { get; set; }
    }
}
