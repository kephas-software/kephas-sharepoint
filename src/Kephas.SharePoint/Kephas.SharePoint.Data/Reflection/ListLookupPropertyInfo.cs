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
    using Microsoft.SharePoint.Client;

    /// <summary>
    /// Information about the list lookup property.
    /// </summary>
    public class ListLookupPropertyInfo : ListPropertyInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListLookupPropertyInfo"/> class.
        /// </summary>
        /// <param name="field">The field.</param>
        protected internal ListLookupPropertyInfo(Field field)
            : base(field)
        {
        }
    }
}
