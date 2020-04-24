// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISiteCredential.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Declares the ISiteCredential interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Security
{
    /// <summary>
    /// Interface for site credential.
    /// </summary>
    public interface ISiteCredential
    {
        /// <summary>
        /// Parses the string and loads the values from the result.
        /// </summary>
        /// <param name="value">The value to parse.</param>
        void Load(string value);
    }
}
