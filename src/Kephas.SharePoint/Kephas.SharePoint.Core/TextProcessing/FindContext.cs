// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindContext.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the find context class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.TextProcessing
{
    using Kephas.Composition;
    using Kephas.Services;

    /// <summary>
    /// A find context.
    /// </summary>
    public class FindContext : Context, IFindContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindContext"/> class.
        /// </summary>
        /// <param name="compositionContext">Context for the composition.</param>
        public FindContext(ICompositionContext compositionContext)
            : base(compositionContext)
        {
        }

        /// <summary>
        /// Gets or sets the occurence. By default, the first occurence is considered.
        /// </summary>
        /// <value>
        /// The occurence.
        /// </value>
        public int Occurence { get; set; } = 1;
    }
}
