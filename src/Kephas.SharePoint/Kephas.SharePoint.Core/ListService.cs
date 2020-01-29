﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListService.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the list service class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using System;
    using System.Linq;

    /// <summary>
    /// A list service.
    /// </summary>
    public class ListService : IListService
    {
        private readonly ISiteSettingsProvider siteSettingsProvider;
        private readonly IDefaultSettingsProvider defaultSettingsProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListService"/> class.
        /// </summary>
        /// <param name="siteSettingsProvider">The site settings provider.</param>
        /// <param name="defaultSettingsProvider">The default settings provider.</param>
        public ListService(
            ISiteSettingsProvider siteSettingsProvider,
            IDefaultSettingsProvider defaultSettingsProvider)
        {
            this.siteSettingsProvider = siteSettingsProvider;
            this.defaultSettingsProvider = defaultSettingsProvider;
        }

        /// <summary>
        /// Gets the default library.
        /// </summary>
        /// <returns>
        /// The default library.
        /// </returns>
        public string GetDefaultLibrary()
        {
            var defaultSettings = this.defaultSettingsProvider.Defaults;
            return string.IsNullOrEmpty(defaultSettings.Site)
                                        ? defaultSettings.Library
                                        : $"{defaultSettings.Site}/{defaultSettings.Library}";
        }

        /// <summary>
        /// Gets the library path fragments.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="listFullName">The list full name, including site.</param>
        /// <returns>
        /// The library path fragments.
        /// </returns>
        public (string siteName, string libraryName) GetListPathFragments(string listFullName)
        {
            var defaultSettings = this.defaultSettingsProvider.Defaults;
            if (string.IsNullOrEmpty(listFullName))
            {
                listFullName = string.IsNullOrEmpty(defaultSettings.Site)
                    ? defaultSettings.Library
                    : $"{defaultSettings.Site}/{defaultSettings.Library}";

                if (string.IsNullOrEmpty(listFullName))
                {
                    throw new InvalidOperationException($"Library full name not provided and the default settings do not specify a library, either.");
                }
            }

            var librarySeparator = listFullName.LastIndexOf('/');
            if (librarySeparator <= 0 && string.IsNullOrEmpty(defaultSettings.Site))
            {
                throw new InvalidOperationException($"Library full name does not contain a site: '{listFullName}', and the default settings do not specify a site, either. Library full names should have the form: <site-name>/<library-name>.");
            }

            var libraryName = librarySeparator <= 0 ? listFullName : listFullName.Substring(librarySeparator + 1);
            var siteName = librarySeparator <= 0 ? defaultSettings.Site : listFullName.Substring(0, librarySeparator);

            return (siteName, libraryName);
        }

        /// <summary>
        /// Gets the site settings.
        /// </summary>
        /// <param name="libraryFullName">Information describing the library.</param>
        /// <returns>
        /// The site settings.
        /// </returns>
        public SiteSettings GetSiteSettings(string libraryFullName)
        {
            var (siteName, libraryName) = this.GetListPathFragments(libraryFullName);
            var sites = this.siteSettingsProvider.GetSiteSettings();
            if (sites == null)
            {
                throw new SharePointException("No 'sites' section in the configuration file.");
            }

            var siteSettings = sites.FirstOrDefault(s => s.name == siteName).settings;
            if (siteSettings == null)
            {
                siteSettings = sites.FirstOrDefault(s => s.settings.SiteUrl == siteName).settings;
            }

            if (siteSettings == null)
            {
                throw new SharePointException($"Site '{siteName}' not found in the configuration file, the 'sites' section.");
            }

            return siteSettings;
        }
    }
}