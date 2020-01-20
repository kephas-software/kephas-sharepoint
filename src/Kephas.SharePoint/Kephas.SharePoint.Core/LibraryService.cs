// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LibraryService.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the library service class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using System;
    using System.Linq;

    using Kephas.Configuration;

    /// <summary>
    /// A library service.
    /// </summary>
    public class LibraryService : ILibraryService
    {
        private readonly IConfiguration<SharePointSettings> sharePointConfig;
        private readonly IDefaultSettingsProvider defaultSettingsProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryService"/> class.
        /// </summary>
        /// <param name="sharePointConfig">The SharePoint configuration.</param>
        /// <param name="defaultSettingsProvider">The default settings provider.</param>
        public LibraryService(
            IConfiguration<SharePointSettings> sharePointConfig,
            IDefaultSettingsProvider defaultSettingsProvider)
        {
            this.sharePointConfig = sharePointConfig;
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
        /// Gets the document library.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <returns>
        /// The document library.
        /// </returns>
        public string GetDocumentLibrary(Document doc)
        {
            string defaultLibrarySpec = this.GetDefaultLibrary();
            var librarySpec = string.IsNullOrEmpty(doc.Library)
                ? defaultLibrarySpec
                : string.IsNullOrEmpty(doc.Site)
                    ? doc.Library
                    : $"{doc.Site}/{doc.Library}";
            return librarySpec;
        }

        /// <summary>
        /// Gets the library path fragments.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="libraryFullName">The library full name, including site.</param>
        /// <returns>
        /// The library path fragments.
        /// </returns>
        public (string siteName, string libraryName) GetLibraryPathFragments(string libraryFullName)
        {
            var defaultSettings = this.defaultSettingsProvider.Defaults;
            if (string.IsNullOrEmpty(libraryFullName))
            {
                libraryFullName = string.IsNullOrEmpty(defaultSettings.Site)
                    ? defaultSettings.Library
                    : $"{defaultSettings.Site}/{defaultSettings.Library}";

                if (string.IsNullOrEmpty(libraryFullName))
                {
                    throw new InvalidOperationException($"Library full name not provided and the default settings do not specify a library, either.");
                }
            }

            var librarySeparator = libraryFullName.LastIndexOf('/');
            if (librarySeparator <= 0 && string.IsNullOrEmpty(defaultSettings.Site))
            {
                throw new InvalidOperationException($"Library full name does not contain a site: '{libraryFullName}', and the default settings do not specify a site, either. Library full names should have the form: <site-name>/<library-name>.");
            }

            var libraryName = librarySeparator <= 0 ? libraryFullName : libraryFullName.Substring(librarySeparator + 1);
            var siteName = librarySeparator <= 0 ? defaultSettings.Site : libraryFullName.Substring(0, librarySeparator);

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
            var (siteName, libraryName) = this.GetLibraryPathFragments(libraryFullName);
            var sites = this.sharePointConfig.Settings.Sites;
            if (sites == null)
            {
                throw new SharePointException("No 'sites' section in the configuration file.");
            }

            if (!sites.TryGetValue(siteName, out var siteSettings))
            {
                siteSettings = sites.FirstOrDefault(s => s.Value.SiteUrl == siteName).Value;
            }

            if (siteSettings == null)
            {
                throw new SharePointException($"Site '{siteName}' not found in the configuration file, the 'sites' section.");
            }

            return siteSettings;
        }
    }
}
