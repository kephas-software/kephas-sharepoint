// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListPropertyInfo.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the list type information class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Reflection
{
    using Kephas.Reflection;
    using Kephas.Reflection.Dynamic;
    using Microsoft.SharePoint.Client;
    using System;

    /// <summary>
    /// Information about the list property.
    /// </summary>
    public class ListPropertyInfo : DynamicPropertyInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListPropertyInfo"/> class.
        /// </summary>
        /// <param name="field">The field.</param>
        protected internal ListPropertyInfo(Field field)
        {
            this.Field = field;
            this.Name = field.InternalName;
            this.ValueType = this.GetPropertyType(field);
        }

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <value>
        /// The field.
        /// </value>
        protected internal Field Field { get; }

        /// <summary>
        /// Gets property type.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>
        /// The property type.
        /// </returns>
        protected virtual ITypeInfo GetPropertyType(Field field)
        {
            switch (field.FieldTypeKind)
            {
                case FieldType.Boolean:
                    return typeof(bool).AsRuntimeTypeInfo();
                case FieldType.DateTime:
                    return typeof(DateTime).AsRuntimeTypeInfo();
                case FieldType.Guid:
                    return typeof(Guid).AsRuntimeTypeInfo();
                case FieldType.Integer:
                    return typeof(int).AsRuntimeTypeInfo();
                case FieldType.Currency:
                    return typeof(decimal).AsRuntimeTypeInfo();
                case FieldType.Number:
                    return typeof(double).AsRuntimeTypeInfo();
                case FieldType.Note:
                case FieldType.Text:
                    return typeof(string).AsRuntimeTypeInfo();
            }

            return typeof(object).AsRuntimeTypeInfo();
        }
    }
}
