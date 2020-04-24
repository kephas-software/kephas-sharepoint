// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalCertificateProvider.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the local certificate provider class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Security
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Diagnostics;
    using Kephas.Operations;
    using Kephas.Services;

    /// <summary>
    /// A local certificate provider.
    /// </summary>
    [ServiceName(ServiceName)]
    public class LocalCertificateProvider : ICertificateProvider
    {
        /// <summary>
        /// Name of the service.
        /// </summary>
        public const string ServiceName = "Local";

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
        public Task<IOperationResult<X509Certificate2?>> GetCertificateAsync(string storeName, string certificate, IContext? context = null, CancellationToken cancellationToken = default)
        {
            return Profiler.WithStopwatchAsync(async () =>
            {
                var wellKnownStoreName = storeName.ToLower();
                if (Enum.TryParse<StoreLocation>(storeName, ignoreCase: true, out var storeLocation))
                {
                    using var store = new X509Store(StoreName.My, storeLocation);
                    store.Open(OpenFlags.ReadOnly);
                    var certs = store.Certificates.Find(X509FindType.FindByThumbprint, certificate, validOnly: false);
                    return certs.Count == 0 ? null : certs[0];
                }

                // currently loading from a directory is not supported.
                return null;
            });
        }
    }
}
