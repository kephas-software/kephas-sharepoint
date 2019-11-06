// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaxFileSizeExceededException.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the maximum file size exceeded exception class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using System;

    /// <summary>
    /// Exception for signalling maximum file size exceeded errors.
    /// </summary>
    public class MaxFileSizeExceededException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaxFileSizeExceededException"/> class.
        /// </summary>
        /// <param name="fileName">Filename of the file.</param>
        /// <param name="fileSize">Size of the file.</param>
        /// <param name="maxSize">The maximum configured size.</param>
        public MaxFileSizeExceededException(string fileName, long fileSize, long maxSize)
            : base(GetMessage(fileName, fileSize, maxSize))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxFileSizeExceededException"/> class.
        /// </summary>
        /// <param name="fileName">Filename of the file.</param>
        /// <param name="fileSize">Size of the file.</param>
        /// <param name="maxSize">The maximum configured size.</param>
        /// <param name="inner">The inner exception.</param>
        public MaxFileSizeExceededException(string fileName, long fileSize, long maxSize, Exception inner)
            : base(GetMessage(fileName, fileSize, maxSize), inner)
        {
        }

        private static string GetMessage(string fileName, long fileSize, long maxSize)
        {
            return $"File '{fileName}' with size {fileSize} exceeds maximum configured size of {maxSize}.";
        }
    }
}