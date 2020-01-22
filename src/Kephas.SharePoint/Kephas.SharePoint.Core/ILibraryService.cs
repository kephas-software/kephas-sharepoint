// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILibraryService.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the ILibraryService interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using Kephas.Services;

    /// <summary>
    /// Interface for library service.
    /// </summary>
    [SingletonAppServiceContract]
    public interface ILibraryService
    {
        /// <summary>
        /// Gets the default library.
        /// </summary>
        /// <returns>
        /// The default library.
        /// </returns>
        string GetDefaultLibrary();

        /// <summary>
        /// Gets the library path fragments.
        /// </summary>
        /// <param name="libraryFullName">The library full name, including site.</param>
        /// <returns>
        /// The library path fragments.
        /// </returns>
        (string siteName, string libraryName) GetLibraryPathFragments(string libraryFullName);

        /// <summary>
        /// Gets the site settings.
        /// </summary>
        /// <param name="libraryFullName">The library full name, including site.</param>
        /// <returns>
        /// The site settings.
        /// </returns>
        SiteSettings GetSiteSettings(string libraryFullName);
    }
}
