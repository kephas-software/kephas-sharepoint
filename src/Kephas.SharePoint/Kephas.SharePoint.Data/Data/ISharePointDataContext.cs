// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISharePointDataContext.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the ISharePointDataContext interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Data
{
    using Kephas.Data;
    using Kephas.SharePoint.Reflection;

    /// <summary>
    /// Interface for SharePoint data context.
    /// </summary>
    public interface ISharePointDataContext : IDataContext
    {
        /// <summary>
        /// Gets the metadata cache.
        /// </summary>
        /// <value>
        /// The metadata cache.
        /// </value>
        ISharePointMetadataCache MetadataCache { get; }
    }
}
