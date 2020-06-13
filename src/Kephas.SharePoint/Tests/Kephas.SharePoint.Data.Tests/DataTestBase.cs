// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataTestBase.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the data test base class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Kephas.Application;
    using Kephas.Composition;
    using Kephas.Cryptography;
    using Kephas.Data;
    using Kephas.Logging;
    using Kephas.Reflection;
    using Kephas.Services;
    using Kephas.Services.Composition;
    using Kephas.SharePoint.Data;
    using Kephas.SharePoint.Security;
    using Kephas.SharePoint.Security.Composition;
    using Kephas.Testing.Composition;
    using Kephas.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using NSubstitute;

    public abstract class DataTestBase : CompositionTestBase
    {
        public override IEnumerable<Assembly> GetDefaultConventionAssemblies()
        {
            return new List<Assembly>(base.GetDefaultConventionAssemblies())
            {
                typeof(IDataContext).Assembly,              // Kephas.Data
                typeof(IListService).Assembly,           // Kephas.SharePoint.Core
                typeof(ISharePointDataContext).Assembly,    // Kephas.SharePoint.Data
            };
        }

        protected static ISiteSettingsProvider GetTestSiteSettingsProvider(string siteName = "test", string siteNamespace = null)
        {
            var testSettings = GetTestSiteSettings(siteNamespace);
            var siteSettingsProvider = Substitute.For<ISiteSettingsProvider>();
            siteSettingsProvider.GetSiteSettings().Returns(
                new List<(string name, SiteSettings settings)> { (siteName, testSettings) });

            return siteSettingsProvider;
        }

        protected static ISiteServiceProvider GetTestSiteServiceProvider(ISiteSettingsProvider siteSettingsProvider, string siteName = "test")
        {
            var clientContextProvider = new ClientContextProvider(GetAuthManagers().ToList());
            var libraryService = new ListService(siteSettingsProvider, new NullDefaultSettingsProvider());
            var siteService = new SiteService(clientContextProvider, libraryService, Substitute.For<ILogManager>());
            var siteServiceProvider = Substitute.For<ISiteServiceProvider>();
            siteServiceProvider.GetDefaultSiteService(Arg.Any<bool>()).Returns(siteService);
            siteServiceProvider.GetSiteService(siteName, Arg.Any<bool>()).Returns(siteService);

            var (_, siteSettings) = siteSettingsProvider.GetSiteSettings().First(s => s.name == siteName);

            var context = new Context(Substitute.For<ICompositionContext>())
                .Set(nameof(ISiteService.SiteName), siteName)
                .Set(nameof(SiteSettings), siteSettings);
            siteService.InitializeAsync(context).WaitNonLocking();

            return siteServiceProvider;
        }

        protected static SiteSettings GetTestSiteSettings(string siteNamespace)
        {
            var appSettings = new ConfigurationBuilder()
                .AddUserSecrets<DataTestBase>()
                .Build();
            var ns = string.IsNullOrEmpty(siteNamespace)
                ? string.Empty
                : $"{siteNamespace}-";
            return new SiteSettings
            {
                SiteUrl = appSettings[$"{ns}SiteSettings:SiteUrl"] ?? "<please provide site url>",
                Credential = GetCredential(appSettings[$"{ns}SiteSettings:CredentialType"], appSettings[$"{ns}SiteSettings:Credential"]),
            };
        }

        private static IEnumerable<Lazy<ISiteAuthenticationManager, SiteAuthenticationManagerMetadata>> GetAuthManagers()
        {
            var encryptionService = Substitute.For<IEncryptionService>();

            yield return new Lazy<ISiteAuthenticationManager, SiteAuthenticationManagerMetadata>(
                () => new PasswordSiteAuthenticationManager(encryptionService),
                new SiteAuthenticationManagerMetadata(typeof(PasswordSiteCredential)));

            yield return new Lazy<ISiteAuthenticationManager, SiteAuthenticationManagerMetadata>(
                () => new AppSiteAuthenticationManager(
                    encryptionService,
                    new List<Lazy<ICertificateProvider, AppServiceMetadata>>
                    {
                        new Lazy<ICertificateProvider, AppServiceMetadata>(
                            () => new LocalCertificateProvider(),
                            new AppServiceMetadata(serviceName: LocalCertificateProvider.ServiceName)),
                    }),
                new SiteAuthenticationManagerMetadata(typeof(AppSiteCredential)));
        }

        private static ISiteCredential GetCredential(string typeName, string credentialString)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException("Credential type name not provided.");
            }

            if (string.IsNullOrEmpty(credentialString))
            {
                throw new ArgumentException("Credential not provided.");
            }

            using var appRuntime = new StaticAppRuntime(defaultAssemblyFilter: an => an.Name.StartsWith("Kephas", StringComparison.OrdinalIgnoreCase));
            ServiceHelper.Initialize(appRuntime);
            var credentialType = new DefaultTypeResolver(appRuntime).ResolveType(typeName);
            var credential = (ISiteCredential)credentialType.AsRuntimeTypeInfo().CreateInstance();
            credential.Load(credentialString);
            return credential;
        }
    }
}
