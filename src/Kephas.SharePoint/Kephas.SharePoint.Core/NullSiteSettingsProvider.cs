// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullSiteSettingsProvider.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the null site settings provider class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using System.Collections.Generic;

    using Kephas.Services;

    /// <summary>
    /// A null site settings provider.
    /// </summary>
    [OverridePriority(Priority.Lowest)]
    public class NullSiteSettingsProvider : ISiteSettingsProvider
    {
        /// <summary>
        /// Gets the site account settings.
        /// </summary>
        /// <returns>
        /// An enumeration of site account name and settings tuples.
        /// </returns>
        public IEnumerable<(string name, SiteAccountSettings settings)> GetAccountSettings()
        {
            yield break;
        }

        /// <summary>
        /// Gets the site settings.
        /// </summary>
        /// <returns>
        /// An enumeration of site name and settings tuples.
        /// </returns>
        public IEnumerable<(string name, SiteSettings settings)> GetSiteSettings()
        {
            yield break;
        }
    }
}
