// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISiteServiceProvider.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the ISiteServiceProvider interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using System;

    using Kephas.Services;

    /// <summary>
    /// Interface for site service provider.
    /// </summary>
    [SingletonAppServiceContract]
    public interface ISiteServiceProvider
    {
        /// <summary>
        /// Gets the default site service.
        /// </summary>
        /// <param name="throwOnNotFound">Optional. True to throw on not found.</param>
        /// <returns>
        /// The default site service.
        /// </returns>
        ISiteService GetDefaultSiteService(bool throwOnNotFound = true);

        /// <summary>
        /// Gets the site service for the provided site name.
        /// </summary>
        /// <param name="siteName">Name of the site.</param>
        /// <param name="throwOnNotFound">Optional. True to throw on not found.</param>
        /// <returns>
        /// The site service.
        /// </returns>
        ISiteService GetSiteService(string siteName, bool throwOnNotFound = true);

        /// <summary>
        /// Gets the site service for the provided site name.
        /// </summary>
        /// <param name="siteId">Name of the site.</param>
        /// <param name="throwOnNotFound">Optional. True to throw on not found.</param>
        /// <returns>
        /// The site service.
        /// </returns>
        ISiteService GetSiteService(Guid siteId, bool throwOnNotFound = true);
    }
}
