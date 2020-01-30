// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFindContext.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the IFindContext interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.TextProcessing
{
    using Kephas.Services;

    /// <summary>
    /// Interface for find context.
    /// </summary>
    public interface IFindContext : IContext
    {
        /// <summary>
        /// Gets or sets the occurence.
        /// </summary>
        /// <value>
        /// The occurence.
        /// </value>
        int Occurence { get; set; }
    }
}
