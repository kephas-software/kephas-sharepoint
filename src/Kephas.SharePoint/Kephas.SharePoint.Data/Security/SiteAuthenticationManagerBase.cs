// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SiteAuthenticationManagerBase.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the site authentication manager base class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Security
{
    using System;
    using System.Security;
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Collections;
    using Kephas.Cryptography;
    using Kephas.Services;
    using Kephas.SharePoint;
    using Microsoft.SharePoint.Client;

    /// <summary>
    /// A site authentication manager base.
    /// </summary>
    /// <typeparam name="TCredential">Type of the credential.</typeparam>
    public abstract class SiteAuthenticationManagerBase<TCredential> : ISiteAuthenticationManager<TCredential>
        where TCredential : ISiteCredential
    {
        private readonly IEncryptionService encryptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteAuthenticationManagerBase{TCredential}"/> class.
        /// </summary>
        /// <param name="encryptionService">The encryption service.</param>
        public SiteAuthenticationManagerBase(IEncryptionService encryptionService)
        {
            this.encryptionService = encryptionService;
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
        public abstract Task<ClientContext> GetAuthenticatedContextAsync(
            SiteSettings settings,
            TCredential credential,
            IContext? context = null,
            CancellationToken cancellationToken = default);

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
        Task<ClientContext> ISiteAuthenticationManager.GetAuthenticatedContextAsync(SiteSettings settings, ISiteCredential credential, IContext? context, CancellationToken cancellationToken)
        {
            if (credential is TCredential typedCredential)
            {
                return this.GetAuthenticatedContextAsync(settings, typedCredential, context, cancellationToken);
            }

            throw new ArgumentException($"The '{nameof(credential)}' argument must be of type '{typeof(TCredential).FullName}'. '{credential?.GetType()} is not supported.'");
        }

        /// <summary>
        /// Gets the secure password.
        /// </summary>
        /// <param name="plainPassword">The plain password.</param>
        /// <param name="encryptedPassword">The encrypted password.</param>
        /// <returns>
        /// The secure password.
        /// </returns>
        protected SecureString? GetSecurePassword(string plainPassword, string encryptedPassword)
        {
            var password = this.GetPassword(plainPassword, encryptedPassword);
            if (password == null)
            {
                return null;
            }

            var securePassword = new SecureString();
            password.ForEach(securePassword.AppendChar);
            return securePassword;
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <param name="plainPassword">The plain password.</param>
        /// <param name="encryptedPassword">The encrypted password.</param>
        /// <returns>
        /// The password.
        /// </returns>
        protected string? GetPassword(string plainPassword, string encryptedPassword)
        {
            return string.IsNullOrEmpty(plainPassword)
                ? string.IsNullOrEmpty(encryptedPassword)
                    ? null
                    : this.encryptionService.Decrypt(encryptedPassword)
                : plainPassword;
        }
    }
}
