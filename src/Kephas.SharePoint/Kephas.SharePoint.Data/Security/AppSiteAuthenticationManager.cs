// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppSiteAuthenticationManager.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the application site authentication manager class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Cryptography;
    using Kephas.Services;
    using Kephas.Services.Composition;
    using Kephas.SharePoint;
    using Kephas.Threading.Tasks;
    using Microsoft.SharePoint.Client;
    using OfficeDevPnP.Core;

    /// <summary>
    /// Manager for application site authentications.
    /// </summary>
    public class AppSiteAuthenticationManager : SiteAuthenticationManagerBase<AppSiteCredential>
    {
        private readonly ICollection<Lazy<ICertificateProvider, AppServiceMetadata>> certificateProviders;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSiteAuthenticationManager"/> class.
        /// </summary>
        /// <param name="encryptionService">The encryption service.</param>
        /// <param name="certificateProviders">The certificate providers.</param>
        public AppSiteAuthenticationManager(
            IEncryptionService encryptionService,
            ICollection<Lazy<ICertificateProvider, AppServiceMetadata>> certificateProviders)
            : base(encryptionService)
        {
            this.certificateProviders = certificateProviders.Order().ToList();
        }

        /// <summary>
        /// Gets the authenticated context asynchronously.
        /// </summary>
        /// <param name="settings">The site settings.</param>
        /// <param name="credential">The credential.</param>
        /// <param name="context">Optional. The context.</param>
        /// <param name="cancellationToken">Optional. A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result that yields the authenticated context.
        /// </returns>
        public override async Task<ClientContext> GetAuthenticatedContextAsync(
            SiteSettings settings,
            AppSiteCredential credential,
            IContext? context = null,
            CancellationToken cancellationToken = default)
        {
            var certProvider = this.certificateProviders.FirstOrDefault(l => l.Metadata.ServiceName.Equals(credential.CertificateProvider, StringComparison.OrdinalIgnoreCase));
            if (certProvider == null)
            {
                throw new SharePointException($"No certificate provider found for '{credential.CertificateProvider}'. Possible cause: bad application configuration.");
            }

            var certificateResult = await certProvider.Value.GetCertificateAsync(credential.CertificateStore, credential.Certificate, cancellationToken: cancellationToken)
                .PreserveThreadContext();

            if (certificateResult.Value == null)
            {
                throw new SharePointException($"No certificate found for '{credential.CertificateStore}/{credential.Certificate}'. Possible cause: bad application configuration.");
            }

            // https://docs.microsoft.com/en-us/sharepoint/dev/solution-guidance/security-apponly-azuread
            return new AuthenticationManager().GetAzureADAppOnlyAuthenticatedContext(
                settings.SiteUrl,
                credential.AppId,
                credential.Domain,
                certificateResult.Value);
        }
    }
}
