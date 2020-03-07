// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientContextProvider.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the client context provider class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using System;
    using System.Net;
    using System.Security;
    using System.Threading.Tasks;

    using Kephas.Collections;
    using Kephas.Cryptography;
    using Kephas.Threading.Tasks;
    using Microsoft.SharePoint.Client;

    using AuthenticationManager = OfficeDevPnP.Core.AuthenticationManager;

    /// <summary>
    /// A client context provider.
    /// </summary>
    public class ClientContextProvider : IClientContextProvider
    {
        private readonly IEncryptionService encryptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientContextProvider"/> class.
        /// </summary>
        /// <param name="encryptionService">The encryption service.</param>
        public ClientContextProvider(IEncryptionService encryptionService)
        {
            this.encryptionService = encryptionService;
        }

        /// <summary>
        /// Gets the client context asynchronously.
        /// </summary>
        /// <param name="settings">Options for controlling the operation.</param>
        /// <returns>
        /// An asynchronous result that yields the client context.
        /// </returns>
        public async Task<ClientContext> GetClientContextAsync(SiteSettings settings)
        {
            // https://www.youtube.com/watch?v=prNlFdHP1ZM
            // https://www.c-sharpcorner.com/article/connect-to-sharepoint-online-site-with-app-only-authentication/
            var am = new AuthenticationManager();
            ClientContext clientContext;

            try
            {
                if (!string.IsNullOrWhiteSpace(settings.AppId))
                {
                    clientContext = am.GetAppOnlyAuthenticatedContext(settings.SiteUrl, settings.AppId, this.GetPassword(settings.AppPassword, settings.AppEncryptedPassword));
                }
                else if (!string.IsNullOrWhiteSpace(settings.UserName))
                {
#if NETSTANDARD2_1
                    clientContext = am.GetSharePointOnlineAuthenticatedContextTenant(settings.SiteUrl, settings.UserName, this.GetSecurePassword(settings.UserPassword, settings.UserEncryptedPassword));
#else
                    // clientContext = am.GetSharePointOnlineAuthenticatedContextTenant(settings.SiteUrl, settings.UserName, this.GetSecurePassword(settings.UserPassword, settings.UserEncryptedPassword));
                    clientContext = am.GetAzureADCredentialsContext(settings.SiteUrl, settings.UserName, this.GetSecurePassword(settings.UserPassword, settings.UserEncryptedPassword));
#endif
                }
                else
                {
                    throw new InvalidOperationException($"Either the application ID and password or the user name and password must be provided to connect to SharePoint.");
                }

                clientContext.Load(clientContext.Web, w => w.ServerRelativeUrl, w => w.Id, w => w.Url);
                clientContext.Load(clientContext.Site, s => s.Url, s => s.Id);
                await clientContext.ExecuteQueryAsync().PreserveThreadContext();

                return clientContext;
            }
            catch (WebException ex)
            {
                if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new SharePointException($"Cannot connect to SharePoint site '{settings.SiteUrl}' with '{settings.AppId ?? settings.UserName}', check whether the user name/password are correct.", ex);
                }

                throw;
            }
        }

        private SecureString GetSecurePassword(string plainPassword, string encryptedPassword)
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

        private string GetPassword(string plainPassword, string encryptedPassword)
        {
            return string.IsNullOrEmpty(plainPassword)
                ? string.IsNullOrEmpty(encryptedPassword)
                    ? null
                    : this.encryptionService.Decrypt(encryptedPassword)
                : plainPassword;
        }
    }
}
