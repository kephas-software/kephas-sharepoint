// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharePointTestBase.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the SharePoint test base class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Core.Tests
{
    using System.Collections.Generic;
    using System.Reflection;

    using Kephas.Testing.Composition;

    public abstract class SharePointTestBase : CompositionTestBase
    {
        public override IEnumerable<Assembly> GetDefaultConventionAssemblies()
        {
            return new List<Assembly>(base.GetDefaultConventionAssemblies())
                    {
                        typeof(IDefaultSettingsProvider).Assembly,            // Kephas.SharePoint.Core
                    };
        }
    }
}
