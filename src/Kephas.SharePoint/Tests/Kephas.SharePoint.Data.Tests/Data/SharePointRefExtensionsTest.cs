// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharePointRefExtensionsTest.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the SharePoint reference extensions test class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Tests.Data
{
    using Kephas.Dynamic;
    using Kephas.SharePoint.Data;
    using NUnit.Framework;

    [TestFixture]
    public class SharePointRefExtensionsTest
    {
        [Test]
        public void ToLookupValue_null_id()
        {
            var spRef = new SharePointRef(new Expando(), "Field");

            Assert.IsNull(spRef.ToLookupValue());
        }

        [Test]
        public void ToLookupValue_null()
        {
            var spRef = new SharePointRef(new Expando(), "Field");

            Assert.IsNull(((SharePointRef)null).ToLookupValue());
        }
    }
}
