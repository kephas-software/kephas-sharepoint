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
    using System.Collections.Generic;
    using System.Linq;

    using Kephas.Reflection;
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
        /// <param name="siteName">Name of the site.</param>
        /// <param name="siteUrl">URL of the site.</param>
        internal ListInfo(List list, string siteName, string siteUrl)
        {
            this.list = list;
            this.Name = list.Title;
            this.FullName = $"{siteName}/{list.Title}";
            this.Namespace = siteUrl;
            this.QualifiedFullName = $"{siteUrl}/{list.Title}";

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
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public object Id => this.list.Id;

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
