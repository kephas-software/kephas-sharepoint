// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IClientContextProvider.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the IClientContextProvider interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint
{
    using System.Threading.Tasks;

    using Kephas.Services;
    using Microsoft.SharePoint.Client;

    /// <summary>
    /// Interface for client context provider.
    /// </summary>
    [SingletonAppServiceContract]
    public interface IClientContextProvider
    {
        /// <summary>
        /// Gets the client context asynchronously.
        /// </summary>
        /// <param name="settings">Options for controlling the operation.</param>
        /// <returns>
        /// An asynchronous result that yields the client context.
        /// </returns>
        Task<ClientContext> GetClientContextAsync(SiteSettings settings);
    }
}
