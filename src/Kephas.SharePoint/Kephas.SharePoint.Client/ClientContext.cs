// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientContext.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the client context class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using Kephas.Composition;
    using Kephas.Services;

    /// <summary>
    /// Provides the client context.
    /// </summary>
    public class ClientContext : Context
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientContext"/> class.
        /// </summary>
        /// <param name="compositionContext">Context for the composition.</param>
        public ClientContext(ICompositionContext compositionContext)
            : base(compositionContext)
        {
        }
    }
}
