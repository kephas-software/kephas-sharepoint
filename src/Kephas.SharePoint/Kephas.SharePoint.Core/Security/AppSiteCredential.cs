// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppSiteCredential.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the application site credential class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Security
{
    using Kephas.Diagnostics.Contracts;

    /// <summary>
    /// An application site credential.
    /// </summary>
    public class AppSiteCredential : ISiteCredential
    {
        /// <summary>
        /// Gets or sets the identifier of the application.
        /// </summary>
        /// <value>
        /// The identifier of the application.
        /// </value>
        public string AppId { get; set; }

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the name or thumbprint of the certificate.
        /// </summary>
        /// <value>
        /// The name or thumbprint of the certificate.
        /// </value>
        public string Certificate { get; set; }

        /// <summary>
        /// Gets or sets the name of the certificate provider.
        /// Typically it is one of: Local, AzureKeyVault.
        /// </summary>
        /// <value>
        /// The certificate provider.
        /// </value>
        public string CertificateProvider { get; set; } = LocalCertificateProvider.ServiceName;

        /// <summary>
        /// Gets or sets the name of the certificate store.
        /// Its form is dependent on the certificate provider.
        /// For the local provider it can be one of: CurrentUser, LocalMachine.
        /// </summary>
        /// <value>
        /// The certificate store.
        /// </value>
        public string CertificateStore { get; set; } = "CurrentUser";

        /// <summary>
        /// Parses the string and loads the values from the result.
        /// </summary>
        /// <param name="value">The value to parse.</param>
        public void Load(string value)
        {
            Requires.NotNullOrEmpty(value, nameof(value));

            var splits = value.Split(',');
            this.AppId = splits[0];
            this.Domain = splits.Length > 1 ? splits[1] : null;
            this.CertificateProvider = splits.Length > 2 ? splits[2] : null;
            this.CertificateStore = splits.Length > 3 ? splits[3] : null;
            this.Certificate = splits.Length > 4 ? splits[4] : null;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return $"{base.ToString()}: {this.AppId}/{this.Domain}, Certificate {this.CertificateProvider}/{this.CertificateStore}/{this.Certificate}";
        }
    }
}
