// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharePointRefExtensions.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the SharePoint reference extensions class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Data
{
    using Kephas.Diagnostics.Contracts;
    using Microsoft.SharePoint.Client;

    /// <summary>
    /// A SharePoint reference extensions.
    /// </summary>
    public static class SharePointRefExtensions
    {
        /// <summary>
        /// Converts a <see cref="SharePointRef"/> to a lookup value.
        /// </summary>
        /// <param name="spRef">The SharePoint reference.</param>
        /// <returns>
        /// <see cref="SharePointRef"/> as a FieldLookupValue.
        /// </returns>
        public static FieldLookupValue? ToLookupValue(this SharePointRef spRef)
        {
            if (spRef?.Id == null)
            {
                return null;
            }

            return new FieldLookupValue() { LookupId = (int)spRef.Id };
        }
    }
}
