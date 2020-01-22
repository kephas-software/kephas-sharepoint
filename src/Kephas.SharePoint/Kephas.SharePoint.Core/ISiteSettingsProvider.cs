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
        /// Gets the site settings in this collection.
        /// </summary>
        /// <returns>
        /// An enumerator that allows foreach to be used to process the site settings in this collection.
        /// </returns>
        IEnumerable<(string name, SiteSettings settings)> GetSiteSettings();
    }
}
