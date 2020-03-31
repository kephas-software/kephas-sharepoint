// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LibrarySettings.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the library settings class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using System.Collections.Generic;

    /// <summary>
    /// A library settings.
    /// </summary>
    public class LibrarySettings
    {
        /// <summary>
        /// Gets or sets the text finder for the library.
        /// </summary>
        /// <value>
        /// The text finder for the library.
        /// </value>
        public string TextFinder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the text content is required for the lookup.
        /// </summary>
        /// <value>
        /// True if the text content is required, false if not.
        /// </value>
        public bool RequireTextContent { get; set; } = true;

        /// <summary>
        /// Gets or sets the library fields.
        /// </summary>
        /// <value>
        /// The library fields.
        /// </value>
        public IDictionary<string, FieldSettings> Fields { get; set; } = new Dictionary<string, FieldSettings>();
    }
}
