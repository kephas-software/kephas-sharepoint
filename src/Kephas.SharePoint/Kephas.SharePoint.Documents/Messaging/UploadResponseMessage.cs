// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadResponseMessage.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Messaging
{
    using Kephas.Messaging.Messages;
    using Kephas.Operations;

    /// <summary>
    /// The response message for the upload operation.
    /// </summary>
    public class UploadResponseMessage : ResponseMessage
    {
        /// <summary>
        /// Gets or sets the operation result.
        /// </summary>
        public IOperationResult? Result { get; set; }
    }
}