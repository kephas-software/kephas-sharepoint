// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultSettingsProvider.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the default settings provider class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using Kephas.Configuration;
    using Kephas.SharePoint.Configuration;

    /// <summary>
    /// A default settings provider.
    /// </summary>
    public class DefaultSettingsProvider : IDefaultSettingsProvider
    {
        private readonly IConfiguration<SharePointSettings> config;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSettingsProvider"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public DefaultSettingsProvider(IConfiguration<SharePointSettings> config)
        {
            this.config = config;
        }

        /// <summary>
        /// Gets the defaults.
        /// </summary>
        /// <value>
        /// The defaults.
        /// </value>
        public DefaultSettings Defaults => this.config.Settings.Defaults;
    }
}
