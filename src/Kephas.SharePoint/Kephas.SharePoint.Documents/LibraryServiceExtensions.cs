// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LibraryServiceExtensions.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the site settings provider class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using Kephas.Diagnostics.Contracts;

    /// <summary>
    /// A library service extensions.
    /// </summary>
    public static class LibraryServiceExtensions
    {
        /// <summary>
        /// Gets the document library.
        /// </summary>
        /// <param name="libraryService">The libraryService to act on.</param>
        /// <param name="doc">The document.</param>
        /// <returns>
        /// The document library.
        /// </returns>
        public static string GetDocumentLibrary(this IListService libraryService, Document doc)
        {
            Requires.NotNull(libraryService, nameof(libraryService));

            string defaultLibrarySpec = libraryService.GetDefaultLibrary();
            var librarySpec = string.IsNullOrEmpty(doc.Library)
                ? defaultLibrarySpec
                : string.IsNullOrEmpty(doc.Site)
                    ? doc.Library
                    : $"{doc.Site}/{doc.Library}";
            return librarySpec;
        }
    }
}
