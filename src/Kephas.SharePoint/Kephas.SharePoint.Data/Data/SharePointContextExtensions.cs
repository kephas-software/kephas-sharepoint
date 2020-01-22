// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharePointContextExtensions.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the SharePoint query operation context extensions class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Data
{
    using Kephas.Data;
    using Kephas.Diagnostics.Contracts;

    /// <summary>
    /// A SharePoint query operation context extensions.
    /// </summary>
    public static class SharePointContextExtensions
    {
        private const string ListFullNameKey = "ListFullName";

        /// <summary>
        /// Sets the list full name.
        /// </summary>
        /// <typeparam name="TContext">Actual type of the query operation context.</typeparam>
        /// <param name="queryContext">The query context.</param>
        /// <param name="listFullName">Full name of the list.</param>
        /// <returns>
        /// This <paramref name="queryContext"/>.
        /// </returns>
        public static TContext ListFullName<TContext>(this TContext queryContext, string listFullName)
            where TContext : class, IQueryOperationContext
        {
            Requires.NotNull(queryContext, nameof(queryContext));

            queryContext[ListFullNameKey] = listFullName;

            return queryContext;
        }

        /// <summary>
        /// Gets the list full name.
        /// </summary>
        /// <typeparam name="TContext">Actual type of the query operation context.</typeparam>
        /// <param name="queryContext">The query context.</param>
        /// <returns>
        /// The list full name.
        /// </returns>
        public static string ListFullName<TContext>(this TContext queryContext)
            where TContext : class, IQueryOperationContext
        {
            Requires.NotNull(queryContext, nameof(queryContext));

            return queryContext[ListFullNameKey] as string;
        }
    }
}
