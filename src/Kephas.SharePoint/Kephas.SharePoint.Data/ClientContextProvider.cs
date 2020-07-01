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
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using Kephas.Services;
    using Kephas.SharePoint.Security;
    using Kephas.SharePoint.Security.Composition;
    using Kephas.Threading.Tasks;
    using Microsoft.SharePoint.Client;

    using AuthenticationManager = OfficeDevPnP.Core.AuthenticationManager;

    /// <summary>
    /// A client context provider.
    /// </summary>
    public class ClientContextProvider : IClientContextProvider
    {
        private readonly IList<Lazy<ISiteAuthenticationManager, SiteAuthenticationManagerMetadata>> authManagers;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientContextProvider"/> class.
        /// </summary>
        /// <param name="authManagers">The authentication managers.</param>
        public ClientContextProvider(ICollection<Lazy<ISiteAuthenticationManager, SiteAuthenticationManagerMetadata>> authManagers)
        {
            this.authManagers = authManagers.Order().ToList();
        }

        /// <summary>
        /// Gets the client context asynchronously.
        /// </summary>
        /// <param name="account">The account settings.</param>
        /// <param name="settings">The site settings.</param>
        /// <returns>
        /// An asynchronous result that yields the client context.
        /// </returns>
        public async Task<ClientContext> GetClientContextAsync(SiteAccountSettings account, SiteSettings settings)
        {
            // https://www.youtube.com/watch?v=prNlFdHP1ZM
            // https://www.c-sharpcorner.com/article/connect-to-sharepoint-online-site-with-app-only-authentication/

            if (account?.Credential == null)
            {
                throw new SharePointException($"The credential setting is missing, cannot connect to '{settings.SiteUrl}'.");
            }

            var credentialType = account.Credential.GetType();
            var authManager = this.authManagers.FirstOrDefault(l => l.Metadata.CredentialType == credentialType)?.Value;
            if (authManager == null)
            {
                throw new SharePointException($"The credential type '{credentialType}' is not supported, check whether you are missing a site authentication manager implementation.");
            }

            try
            {
                var clientContext = await authManager.GetAuthenticatedContextAsync(settings, account.Credential).PreserveThreadContext();

                clientContext.Load(clientContext.Web, w => w.ServerRelativeUrl, w => w.Id, w => w.Url);
                clientContext.Load(clientContext.Site, s => s.Url, s => s.Id);
                await clientContext.ExecuteQueryAsync().PreserveThreadContext();

                return clientContext;
            }
            catch (WebException ex)
            {
                if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new SharePointException(
                        $"Cannot connect to SharePoint site '{settings.SiteUrl}' with '{account.Credential}', check whether the credentials are correct.",
                        ex);
                }

                throw;
            }
        }
    }
}