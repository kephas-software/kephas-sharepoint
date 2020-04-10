// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IListPropertyInfo.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the IListPropertyInfo interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Reflection
{
    using Kephas.Reflection;

    /// <summary>
    /// Interface for property information.
    /// </summary>
    public interface IListPropertyInfo : IPropertyInfo
    {
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; }

        /// <summary>
        /// Gets the property kind.
        /// </summary>
        /// <value>
        /// The property kind.
        /// </value>
        public ListPropertyKind Kind { get; }
    }
}
