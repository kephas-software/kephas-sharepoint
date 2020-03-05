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
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Kephas.Composition;
    using Kephas.Cryptography;
    using Kephas.Data;
    using Kephas.Logging;
    using Kephas.Services;
    using Kephas.SharePoint.Data;
    using Kephas.Testing.Composition;
    using Kephas.Threading.Tasks;
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

        protected static ISiteSettingsProvider GetTestSiteSettingsProvider(string siteName = "test")
        {
            var testSettings = GetTestSiteSettings();
            var siteSettingsProvider = Substitute.For<ISiteSettingsProvider>();
            siteSettingsProvider.GetSiteSettings().Returns(
                new List<(string name, SiteSettings settings)> { (siteName, testSettings) });

            return siteSettingsProvider;
        }

        protected static ISiteServiceProvider GetTestSiteServiceProvider(ISiteSettingsProvider siteSettingsProvider, string siteName = "test")
        {
            var clientContextProvider = new ClientContextProvider(Substitute.For<IEncryptionService>());
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

        protected static SiteSettings GetTestSiteSettings()
        {
            return new SiteSettings
            {
                SiteUrl = "<site>",
                UserName = "<user>",
                UserPassword = "<clear-text-pwd>",
            };
        }
    }
}
