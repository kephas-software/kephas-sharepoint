// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharePointRef.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the SharePoint reference class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Kephas.SharePoint.Data
{
    using Kephas.Data;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A SharePoint reference.
    /// </summary>
    public class SharePointRef : Ref<ISharePointEntity>
    {
        private object id;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointRef"/> class.
        /// </summary>
        /// <param name="containerEntity">The container entity.</param>
        public SharePointRef(object containerEntity)
            : base(containerEntity, nameof(IIdentifiable.Id))
        {
        }

        /// <summary>
        /// Gets or sets the identifier of the referenced entity.
        /// </summary>
        /// <value>
        /// The identifier of the referenced entity.
        /// </value>
        public override object Id
        {
            get => this.id;
            set => this.id = value;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value { get; set; }

        /// <summary>
        /// Gets the referenced entity asynchronously.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when the requested operation is not supported.</exception>
        /// <param name="throwIfNotFound">Optional. If true and the referenced entity is not found, an
        ///                               exception occurs.</param>
        /// <param name="cancellationToken">Optional. The cancellation token (optional).</param>
        /// <returns>
        /// A task promising the referenced entity.
        /// </returns>
        public override Task<ISharePointEntity> GetAsync(bool throwIfNotFound = true, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException($"Cannot get the reference in this way.");
        }
    }
}
