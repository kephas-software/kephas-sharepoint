// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDefaultSettingsProvider.cs" company="Kephas Software SRL">
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
    /// Interface for default settings provider.
    /// </summary>
    [SingletonAppServiceContract]
    public interface IDefaultSettingsProvider
    {
        /// <summary>
        /// Gets the defaults.
        /// </summary>
        /// <value>
        /// The defaults.
        /// </value>
        DefaultSettings Defaults { get; }
    }
}
