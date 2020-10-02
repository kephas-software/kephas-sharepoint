// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISiteSettingsProvider.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the ISiteSettingsProvider interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Kephas.SharePoint
{
    using System.Collections.Generic;

    using Kephas.Services;

    /// <summary>
    /// Interface for site settings provider.
    /// </summary>
    [SingletonAppServiceContract]
    public interface ISiteSettingsProvider
    {
        /// <summary>
        /// Gets the site settings.
        /// </summary>
        /// <returns>
        /// An enumeration of site name and settings tuples.
        /// </returns>
        IEnumerable<(string name, SiteSettings settings)> GetSiteSettings();

        /// <summary>
        /// Gets the site account settings.
        /// </summary>
        /// <returns>
        /// An enumeration of site account name and settings tuples.
        /// </returns>
        IEnumerable<(string name, SiteAccountSettings settings)> GetAccountSettings();

        /// <summary>
        /// Gets the site account settings for the provided site.
        /// </summary>
        /// <param name="siteName">The site name.</param>
        /// <param name="accountName">Optional. The account name. If none is provided, the account name configured in the site settings will be used.</param>
        /// <returns>The site account settings.</returns>
        SiteAccountSettings? GetSiteAccountSettings(string siteName, string? accountName = null);
    }
}
