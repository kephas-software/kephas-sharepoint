// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListLookupPropertyInfo.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the list type information class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Reflection
{
    using System;

    using Microsoft.SharePoint.Client;

    /// <summary>
    /// Information about the list lookup property.
    /// </summary>
    public class ListLookupPropertyInfo : ListPropertyInfo, IListLookupPropertyInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListLookupPropertyInfo"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
        /// <param name="field">The field.</param>
        /// <param name="declaringContainer">The declaring container.</param>
        protected internal ListLookupPropertyInfo(Field field, IListInfo declaringContainer)
            : base(field, declaringContainer)
        {
            if (!(field is FieldLookup fieldLookup))
            {
                throw new InvalidOperationException($"Only lookup fields supported, instead provided as {field.GetType()}");
            }

            this.RefSiteId = fieldLookup.LookupWebId;
            this.RefListId = fieldLookup.LookupList;
            this.RefField = fieldLookup.LookupField;
        }

        /// <summary>
        /// Gets the identifier of the site.
        /// </summary>
        /// <value>
        /// The identifier of the site.
        /// </value>
        public Guid RefSiteId { get; }

        /// <summary>
        /// Gets the identifier of the referenced list.
        /// </summary>
        /// <value>
        /// The identifier of the referenced list.
        /// </value>
        public string RefListId { get; }

        /// <summary>
        /// Gets the reference field.
        /// </summary>
        /// <value>
        /// The reference field.
        /// </value>
        public string RefField { get; }
    }
}
