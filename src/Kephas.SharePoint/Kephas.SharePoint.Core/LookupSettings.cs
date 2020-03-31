// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LookupSettings.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the lookup settings class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A lookup settings.
    /// </summary>
    public class LookupSettings
    {
        /// <summary>
        /// Gets or sets the default text finder.
        /// </summary>
        /// <value>
        /// The default text finder.
        /// </value>
        public string DefaultTextFinder { get; set; }

        /// <summary>
        /// Gets or sets the strategy when a referenced entry is not found.
        /// </summary>
        /// <value>
        /// The not found strategy.
        /// </value>
        public string RefEntryNotFoundStrategy { get; set; }

        /// <summary>
        /// Gets or sets the cache expiration interval.
        /// </summary>
        /// <value>
        /// The cache expiration interval.
        /// </value>
        public TimeSpan? CacheExpirationInterval { get; set; } = TimeSpan.FromHours(1);

        /// <summary>
        /// Gets or sets the document libraries.
        /// </summary>
        /// <value>
        /// The document libraries.
        /// </value>
        public IDictionary<string, LibrarySettings> Libraries { get; set; } = new Dictionary<string, LibrarySettings>();

        /// <summary>
        /// Gets or sets the lists.
        /// </summary>
        /// <value>
        /// The lists.
        /// </value>
        public IDictionary<string, LookupListSettings> LookupLists { get; set; } = new Dictionary<string, LookupListSettings>();
    }
}
