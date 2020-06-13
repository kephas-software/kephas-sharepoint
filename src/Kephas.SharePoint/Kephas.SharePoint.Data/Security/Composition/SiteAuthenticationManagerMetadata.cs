// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SiteAuthenticationManagerMetadata.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the site authentication manager metadata class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Security.Composition
{
    using System;
    using System.Collections.Generic;

    using Kephas.Collections;
    using Kephas.Services.Composition;

    /// <summary>
    /// A site authentication manager metadata.
    /// </summary>
    public class SiteAuthenticationManagerMetadata : AppServiceMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteAuthenticationManagerMetadata"/> class.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        public SiteAuthenticationManagerMetadata(IDictionary<string, object?> metadata)
            : base(metadata)
        {
            if (metadata == null)
            {
                return;
            }

            this.CredentialType = (Type?)metadata.TryGetValue(nameof(this.CredentialType));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteAuthenticationManagerMetadata"/> class.
        /// </summary>
        /// <param name="credentialType">The type of the credential.</param>
        /// <param name="processingPriority">Optional. The processing priority.</param>
        /// <param name="overridePriority">Optional. The override priority.</param>
        /// <param name="serviceName">Optional. Name of the service.</param>
        public SiteAuthenticationManagerMetadata(Type credentialType, int processingPriority = 0, int overridePriority = 0, string? serviceName = null)
            : base(processingPriority, overridePriority, serviceName)
        {
            this.CredentialType = credentialType;
        }

        /// <summary>
        /// Gets the type of the credential.
        /// </summary>
        /// <value>
        /// The type of the credential.
        /// </value>
        public Type? CredentialType { get; }
    }
}
