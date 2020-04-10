// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListItem.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the list item class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using Kephas.Dynamic;

    /// <summary>
    /// A list item.
    /// </summary>
    public class ListItem : Expando
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }

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
        /// Gets or sets the containing list.
        /// </summary>
        /// <value>
        /// The containing list.
        /// </value>
        public virtual string List { get; set; }

        /// <summary>
        /// Gets or sets the pathname of the containing folder.
        /// </summary>
        /// <value>
        /// The full pathname of the containing folder.
        /// </value>
        public string Folder { get; set; }

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
            return $"{this.List}/{folder}{this.Title}/{this.Id}";
        }
    }
}
