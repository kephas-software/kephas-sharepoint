// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListServiceExtensions.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using Kephas.Diagnostics.Contracts;

    /// <summary>
    /// A list service extensions.
    /// </summary>
    public static class ListServiceExtensions
    {
        /// <summary>
        /// Gets the containing list.
        /// </summary>
        /// <param name="listService">The list service.</param>
        /// <param name="listItem">The list item.</param>
        /// <returns>
        /// The document library.
        /// </returns>
        public static string GetContainingList(this IListService listService, ListItem listItem)
        {
            Requires.NotNull(listService, nameof(listService));

            string defaultLibrarySpec = listService.GetDefaultLibrary();
            var librarySpec = string.IsNullOrEmpty(listItem.List)
                ? defaultLibrarySpec
                : string.IsNullOrEmpty(listItem.Site)
                    ? listItem.List
                    : $"{listItem.Site}/{listItem.List}";
            return librarySpec;
        }
    }
}
