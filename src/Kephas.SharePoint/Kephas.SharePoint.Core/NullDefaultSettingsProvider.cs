// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullDefaultSettingsProvider.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the IDefaultSettingsProvider interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using Kephas.Services;
    using Kephas.SharePoint.Configuration;

    /// <summary>
    /// A null default settings provider.
    /// </summary>
    [OverridePriority(Priority.Lowest)]
    public class NullDefaultSettingsProvider : IDefaultSettingsProvider
    {
        /// <summary>
        /// Gets the defaults.
        /// </summary>
        /// <value>
        /// The defaults.
        /// </value>
        public DefaultSettings Defaults { get; } = new DefaultSettings();
    }
}
