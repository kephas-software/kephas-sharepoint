// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindResult.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the find result class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.TextProcessing
{
    using Kephas.Operations;

    /// <summary>
    /// Encapsulates the result of a find operation.
    /// </summary>
    public class FindResult : OperationResult<bool>, IFindResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindResult"/> class.
        /// </summary>
        public FindResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FindResult"/> class.
        /// </summary>
        /// <param name="returnValue">True to return value.</param>
        public FindResult(bool returnValue)
            : base(returnValue)
        {
        }
    }
}
