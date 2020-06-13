// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICertificateProvider.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the ICertificateProvider interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Security
{
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Operations;
    using Kephas.Services;

    /// <summary>
    /// Interface for certificate provider.
    /// </summary>
    [SingletonAppServiceContract(AllowMultiple = true)]
    public interface ICertificateProvider
    {
        /// <summary>
        /// Gets the certificate asynchronously.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="certificate">The certificate.</param>
        /// <param name="context">Optional. The context.</param>
        /// <param name="cancellationToken">Optional. A token that allows processing to be cancelled.</param>
        /// <returns>
        /// An asynchronous result that yields the certificate.
        /// </returns>
        Task<IOperationResult<X509Certificate2?>> GetCertificateAsync(string storeName, string certificate, IContext? context = null, CancellationToken cancellationToken = default);
    }
}
