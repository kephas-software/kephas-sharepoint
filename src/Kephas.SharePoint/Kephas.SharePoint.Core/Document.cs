// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Document.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the document class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using Kephas.Dynamic;

    /// <summary>
    /// A document.
    /// </summary>
    public class Document : Expando
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the pathname of the containing folder.
        /// </summary>
        /// <value>
        /// The full pathname of the containing folder.
        /// </value>
        public string Folder { get; set; }

        /// <summary>
        /// Gets or sets the name of the original file.
        /// </summary>
        /// <value>
        /// The name of the original file.
        /// </value>
        public string OriginalName { get; set; }

        /// <summary>
        /// Gets or sets the pathname of the original folder.
        /// </summary>
        /// <value>
        /// The pathname of the original folder.
        /// </value>
        public string OriginalFolder { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        /// <value>
        /// The site.
        /// </value>
        public string Site { get; set; }

        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        /// <value>
        /// The library.
        /// </value>
        public string Library { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public byte[] FileContent { get; set; }

        /// <summary>
        /// Gets or sets the text content.
        /// </summary>
        /// <value>
        /// The text content.
        /// </value>
        public string TextContent { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the parent.
        /// </summary>
        /// <value>
        /// The identifier of the parent.
        /// </value>
        public long? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        /// <value>
        /// The group.
        /// </value>
        public string Group { get; set; }

        /// <summary>
        /// Gets or sets the search keywords.
        /// </summary>
        /// <value>
        /// The search keywords.
        /// </value>
        public string[] Keywords { get; set; }

        /// <summary>
        /// Gets or sets the document properties.
        /// </summary>
        /// <value>
        /// The document properties.
        /// </value>
        public IExpando Fields { get; set; } = new Expando();

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            var folder = string.IsNullOrEmpty(this.Folder) ? string.Empty : $"{this.Folder}/";
            return $"{this.Library}/{folder}{this.OriginalName}/{this.Id} ({this.FileContent?.Length} bytes)";
        }
    }
}
