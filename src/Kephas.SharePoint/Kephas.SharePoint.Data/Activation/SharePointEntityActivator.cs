// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharePointEntityActivator.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the SharePoint entity activator class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Activation
{
    using System.Collections.Generic;

    using Kephas.Activation;
    using Kephas.Reflection;
    using Kephas.SharePoint.Data;

    /// <summary>
    /// A SharePoint entity activator.
    /// </summary>
    public class SharePointEntityActivator : ActivatorBase
    {
        /// <summary>
        /// Gets the supported implementation types.
        /// </summary>
        /// <returns>
        /// An enumeration of implementation types.
        /// </returns>
        protected override IEnumerable<ITypeInfo> GetImplementationTypes()
        {
            yield return typeof(SharePointEntity).AsRuntimeTypeInfo();
        }
    }
}
