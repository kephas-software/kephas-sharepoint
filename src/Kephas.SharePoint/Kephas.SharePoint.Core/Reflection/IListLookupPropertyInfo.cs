// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IListLookupPropertyInfo.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the IListLookupPropertyInfo interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Reflection
{
    using System;

    /// <summary>
    /// Interface for lookup property information.
    /// </summary>
    public interface IListLookupPropertyInfo : IListPropertyInfo
    {
        /// <summary>
        /// Gets the identifier of the site.
        /// </summary>
        /// <value>
        /// The identifier of the site.
        /// </value>
        Guid RefSiteId { get; }

        /// <summary>
        /// Gets the identifier of the referenced list.
        /// </summary>
        /// <value>
        /// The identifier of the referenced list.
        /// </value>
        string RefListId { get; }

        /// <summary>
        /// Gets the reference field.
        /// </summary>
        /// <value>
        /// The reference field.
        /// </value>
        string RefField { get; }
    }
}
