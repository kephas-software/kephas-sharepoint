// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RedirectPatternAttribute.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Sources.AttributedModel
{
    using System;

    using Kephas.Composition.Metadata;
    using Kephas.Diagnostics.Contracts;

    /// <summary>
    /// Attribute for specifying the redirect pattern.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class RedirectPatternAttribute : Attribute, IMetadataValue<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectPatternAttribute"/> class.
        /// </summary>
        /// <param name="value">The redirect pattern.</param>
        public RedirectPatternAttribute(string value)
        {
            Requires.NotNullOrEmpty(value, nameof(value));

            this.Value = value;
        }

        /// <summary>
        /// Gets the metadata value.
        /// </summary>
        object IMetadataValue.Value => this.Value;

        /// <summary>
        /// Gets the metadata value.
        /// </summary>
        public string Value { get; }
    }
}