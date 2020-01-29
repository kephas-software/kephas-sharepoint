// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListInfo.cs" company="Kephas Software SRL">
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
    using System.Collections.Generic;
    using System.Linq;

    using Kephas.Reflection.Dynamic;
    using Microsoft.SharePoint.Client;

    /// <summary>
    /// Information about the list type.
    /// </summary>
    public class ListInfo : DynamicTypeInfo, IListInfo
    {
        private readonly List list;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListInfo"/> class.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="siteUrl">URL of the site.</param>
        internal ListInfo(List list, Uri siteUrl)
        {
            this.list = list;
            this.Namespace = siteUrl.ToString();
            this.QualifiedFullName = this.FullName = $"{this.Namespace}/{list.Title}";

            foreach (var field in list.Fields)
            {
                this.AddMember(this.CreatePropertyInfo(field));
            }
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public new IEnumerable<IListPropertyInfo> Properties => base.Properties.OfType<IListPropertyInfo>();

        /// <summary>
        /// Creates property information.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>
        /// The new property information.
        /// </returns>
        protected virtual ListPropertyInfo CreatePropertyInfo(Field field)
        {
            if (field.FieldTypeKind == FieldType.Lookup)
            {
                return new ListLookupPropertyInfo(field);
            }

            return new ListPropertyInfo(field);
        }
    }
}
