// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LookupListSettings.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the lookup list settings class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    /// <summary>
    /// A lookup list settings.
    /// </summary>
    public class LookupListSettings
    {
        /// <summary>
        /// Gets or sets the reference fields.
        /// </summary>
        /// <value>
        /// The reference fields.
        /// </value>
        public string[] RefFields { get; set; } = new[] { "Title", "Name" };

        /// <summary>
        /// Gets or sets the keyword fields used for the lookup.
        /// </summary>
        /// <value>
        /// The keyword fields.
        /// </value>
        public string[] KeywordFields { get; set; } = new[] { "Title", "Aliases" };
    }
}
