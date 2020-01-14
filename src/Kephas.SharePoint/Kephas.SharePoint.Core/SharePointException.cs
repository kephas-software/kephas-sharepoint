// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharePointException.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the SharePoint exception class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using System;

    /// <summary>
    /// Exception for signalling SharePoint errors.
    /// </summary>
    public class SharePointException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SharePointException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public SharePointException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
