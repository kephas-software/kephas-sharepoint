// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISharePointEntity.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the ISharePointEntity interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Data
{
    using Kephas.Data;
    using Kephas.Dynamic;
    using Kephas.SharePoint.Reflection;

    /// <summary>
    /// Interface for SharePoint entity.
    /// </summary>
    public interface ISharePointEntity : IEntity, IExpando, IInstance<IListInfo>
    {
    }
}
