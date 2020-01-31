// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharePointRefTest.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the SharePoint reference test class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Core.Tests.Data
{
    using Kephas.Dynamic;
    using Kephas.SharePoint.Data;
    using NUnit.Framework;

    [TestFixture]
    public class SharePointRefTest
    {
        [Test]
        public void SharePointRef_ID_and_value()
        {
            var doc = new Expando();
            var spRef = new SharePointRef(doc, "Parent");

            spRef.Id = 3;
            spRef.Value = "hello";

            Assert.AreEqual(3, spRef.Id);
            Assert.AreEqual("hello", spRef.Value);
        }
    }
}
