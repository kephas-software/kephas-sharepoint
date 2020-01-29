﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDocumentSource.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the IDocumentSource interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Sources
{
    using Kephas.Services;

    /// <summary>
    /// Interface for a document source.
    /// </summary>
    [SingletonAppServiceContract(AllowMultiple = true)]
    public interface IDocumentSource : IAsyncInitializable, IAsyncFinalizable
    {
    }
}