// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISiteAuthenticationManager.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the ISiteAuthenticationManager interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Security
{
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Services;
    using Microsoft.SharePoint.Client;

    /// <summary>
    /// Interface for site authentication manager.
    /// </summary>
    public interface ISiteAuthenticationManager
    {
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
        Task<ClientContext> GetAuthenticatedContextAsync(
            SiteSettings settings,
            ISiteCredential credential,
            IContext? context = null,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Interface for site authentication manager.
    /// </summary>
    /// <typeparam name="TCredential">Type of the credential.</typeparam>
    [SingletonAppServiceContract(ContractType = typeof(ISiteAuthenticationManager))]
    public interface ISiteAuthenticationManager<TCredential> : ISiteAuthenticationManager
        where TCredential : ISiteCredential
    {
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
        Task<ClientContext> GetAuthenticatedContextAsync(
            SiteSettings settings,
            TCredential credential,
            IContext? context = null,
            CancellationToken cancellationToken = default);
    }
}
