// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharePointMetadataCacheTest.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the SharePoint metadata cache test class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Tests.Reflection
{
    using System.Threading.Tasks;

    using Kephas.SharePoint.Reflection;
    using NUnit.Framework;

    [TestFixture]
    public class SharePointMetadataCacheTest : DataTestBase
    {
        [Test]
        public void ListIdentity_Equals_typed()
        {
            var lid1 = new SharePointMetadataCache.ListIdentity("a");
            var lid2 = new SharePointMetadataCache.ListIdentity("A");
            Assert.IsTrue(lid1.Equals(lid2), "List identities should be equal.");
            Assert.AreEqual(lid1, lid2);
        }

        [Test]
        public void ListIdentity_Equals_object()
        {
            var lid1 = new SharePointMetadataCache.ListIdentity("a");
            var lid2 = new SharePointMetadataCache.ListIdentity("A");
            Assert.IsTrue(((object)lid1).Equals(lid2), "List identities should be equal.");
            Assert.AreEqual(lid1, lid2);
        }

        [Test]
        [TestCase("test/Unsorted", (string)null)]
        [TestCase("test/Unsorted", "app")]
        [TestCase("test/Lieferanten", "sc")]
        [TestCase("test/Lieferanten", "scapp")]
        public async Task GetListInfoAsync_unsorted(string listName, string siteNamespace)
        {
            var settingsProvider = GetTestSiteSettingsProvider(siteNamespace: siteNamespace);
            var provider = GetTestSiteServiceProvider(settingsProvider);

            var cache = new SharePointMetadataCache(new ListService(settingsProvider, new NullDefaultSettingsProvider()), provider);
            var listInfo = await cache.GetListInfoAsync(listName);

            Assert.IsTrue(listName.EndsWith(listInfo.Name, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}
