// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharePointDataContextTest.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the SharePoint data context test class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Tests.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Kephas.Composition;
    using Kephas.Composition.ExportFactories;
    using Kephas.Data;
    using Kephas.Data.Store;
    using Kephas.Services;
    using Kephas.Services.Composition;
    using Kephas.SharePoint;
    using Kephas.SharePoint.Activation;
    using Kephas.SharePoint.Data;
    using Kephas.SharePoint.Reflection;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SharePointDataContextTest : DataTestBase
    {
        [Test]
        public async Task Query()
        {
            var container = this.CreateContainer();

            var siteSettingsProvider = GetTestSiteSettingsProvider();
            var (siteName, siteSettings) = siteSettingsProvider.GetSiteSettings().First();

            var siteServiceProvider = GetTestSiteServiceProvider(siteSettingsProvider);

            var libraryService = new ListService(siteSettingsProvider, new NullDefaultSettingsProvider());
            var spMetadataCache = new SharePointMetadataCache(libraryService, siteServiceProvider);
            var dataContext = new SharePointDataContext(container, spMetadataCache, siteServiceProvider, libraryService);
            var dataStore = Substitute.For<IDataStore>();
            dataStore.EntityActivator.Returns(new SharePointEntityActivator());
            var initContext = new DataInitializationContext(dataContext, dataStore);
            dataContext.Initialize(initContext);

            var query = dataContext.Query<ISharePointEntity>($"{siteName}/Unsorted");

            var results = query.ToList();

            Assert.IsNotNull(results);

            var entity = results.FirstOrDefault();
            if (entity == null)
            {
                return;
            }

            var listInfo = entity.GetTypeInfo();
        }

        [Test]
        public async Task Query_over_data_space()
        {
            var container = this.CreateContainer(parts: new Type[] { typeof(TestSiteSettingsProvider) });

            var siteSettingsProvider = container.GetExport<ISiteSettingsProvider>();
            var (siteName, siteSettings) = siteSettingsProvider.GetSiteSettings().First();
            var siteServiceProvider = container.GetExport<ISiteServiceProvider>();

            await ServiceHelper.InitializeAsync(siteServiceProvider);

            var dataStoreProvider = new DefaultDataStoreProvider(new List<IExportFactory<IDataStoreMatcher, AppServiceMetadata>>
            {
                new ExportFactory<IDataStoreMatcher, AppServiceMetadata>(() => new SharePointDataStoreMatcher(siteSettingsProvider), new AppServiceMetadata()),
            });

            IDataSpace dataSpace = new DataSpace(container, container.GetExport<IDataContextFactory>(), dataStoreProvider);
            dataSpace.Initialize(new Context(container));

            var dataContext = dataSpace[typeof(ISharePointEntity)];

            var query = dataContext.Query<ISharePointEntity>($"{siteName}/Unsorted");

            var results = query.ToList();

            Assert.IsNotNull(results);
        }

        [OverridePriority(Priority.High)]
        public class TestSiteSettingsProvider : ISiteSettingsProvider
        {
            private ISiteSettingsProvider innerProvider;

            public TestSiteSettingsProvider()
            {
                this.innerProvider = GetTestSiteSettingsProvider();
            }

            public IEnumerable<(string name, SiteSettings settings)> GetSiteSettings() => this.innerProvider.GetSiteSettings();
        }
    }
}
