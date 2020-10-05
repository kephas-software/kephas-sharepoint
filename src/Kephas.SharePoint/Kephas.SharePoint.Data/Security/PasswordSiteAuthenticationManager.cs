// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordSiteAuthenticationManager.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the password site authentication manager class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Security
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Cryptography;
    using Kephas.Services;
    using Kephas.Threading.Tasks;
    using Microsoft.SharePoint.Client;

    using AuthenticationManager = OfficeDevPnP.Core.AuthenticationManager;

    /// <summary>
    /// Manager for password site authentications.
    /// </summary>
    public class PasswordSiteAuthenticationManager : SiteAuthenticationManagerBase<PasswordSiteCredential>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordSiteAuthenticationManager"/> class.
        /// </summary>
        /// <param name="encryptionService">The encryption service.</param>
        public PasswordSiteAuthenticationManager(IEncryptionService encryptionService)
            : base(encryptionService)
        {
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
            PasswordSiteCredential credential,
            IContext? context = null,
            CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            return new AuthenticationManager().GetAzureADCredentialsContext(
                settings.SiteUrl,
                credential.UserName,
                this.GetSecurePassword(credential.UserPassword, credential.UserEncryptedPassword));
        }
    }
}
