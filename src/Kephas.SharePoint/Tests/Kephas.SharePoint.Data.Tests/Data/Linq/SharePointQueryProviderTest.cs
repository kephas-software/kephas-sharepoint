// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharePointQueryProviderTest.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the SharePoint query provider class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Tests.Data.Linq
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Kephas.Data;
    using Kephas.Data.Capabilities;
    using Kephas.SharePoint.Data;
    using Kephas.SharePoint.Data.Linq;
    using Kephas.SharePoint.Reflection;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SharePointQueryProviderTest : DataTestBase
    {
        [Test]
        [TestCase((string?)null, "Unsorted")]
        [TestCase("app", "Unsorted")]
        [TestCase("sc", "Unsorted")]
        [TestCase("scapp", "Unsorted")]
        public void Execute(string siteNamespace, string listName)
        {
            var container = this.CreateContainer();

            var siteSettingsProvider = GetTestSiteSettingsProvider(siteNamespace: siteNamespace);
            var (siteName, siteSettings) = siteSettingsProvider.GetSiteSettings().First();

            var siteServiceProvider = GetTestSiteServiceProvider(siteSettingsProvider);

            var dataContext = Substitute.For<IDataContext>();
            dataContext.Attach(Arg.Any<object>()).Returns(ci => new EntityEntry(ci.Arg<object>()));
            var queryContext = new QueryOperationContext(dataContext).ListFullName($"{siteName}/{listName}");

            var provider = new SharePointQueryProvider(
                queryContext,
                new ListService(siteSettingsProvider, new NullDefaultSettingsProvider()),
                siteServiceProvider.GetDefaultSiteService(),
                Substitute.For<IListInfo>());

            var results = provider.Execute<IEnumerable<ISharePointEntity>>(Substitute.For<Expression>()).ToList();

            Assert.IsNotNull(results);
        }
    }
}
