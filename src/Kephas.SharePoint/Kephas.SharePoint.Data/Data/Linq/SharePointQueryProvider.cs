﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SharePointQueryProvider.cs" company="Kephas Software SRL">
//   Copyright (c) Kephas Software SRL. All rights reserved.
//   Licensed under the KEPHAS license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Implements the SharePoint query provider class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Kephas.SharePoint.Data.Linq
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Kephas.Data;
    using Kephas.Data.Linq;
    using Kephas.Diagnostics.Contracts;
    using Kephas.Reflection;
    using Kephas.SharePoint;
    using Kephas.SharePoint.Reflection;
    using Kephas.Threading.Tasks;
    using Microsoft.SharePoint.Client;

    /// <summary>
    /// A SharePoint query provider.
    /// </summary>
    public class SharePointQueryProvider : DataContextQueryProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointQueryProvider"/> class.
        /// </summary>
        /// <param name="queryOperationContext">Context for the query operation.</param>
        /// <param name="listService">The list service.</param>
        /// <param name="siteService">The site service.</param>
        /// <param name="listTypeInfo">Information describing the list type.</param>
        public SharePointQueryProvider(IQueryOperationContext queryOperationContext, IListService listService, ISiteService siteService, IListTypeInfo listTypeInfo)
            : base(queryOperationContext, new InternalQueryProvider())
        {
            this.ListService = listService;
            this.SiteService = siteService;
            this.ListTypeInfo = listTypeInfo;

            var internalQueryProvider = (InternalQueryProvider)this.NativeQueryProvider;
            internalQueryProvider.Provider = this;
        }

        /// <summary>
        /// Gets the list service.
        /// </summary>
        /// <value>
        /// The list service.
        /// </value>
        public IListService ListService { get; }

        /// <summary>
        /// Gets the site service.
        /// </summary>
        /// <value>
        /// The site service.
        /// </value>
        public ISiteService SiteService { get; }

        /// <summary>
        /// Gets information describing the list type.
        /// </summary>
        /// <value>
        /// Information describing the list type.
        /// </value>
        public IListTypeInfo ListTypeInfo { get; }

        private class InternalQueryProvider : IQueryProvider
        {
            /// <summary>
            /// The generic method of IQueryable.CreateQuery{TElement}.
            /// </summary>
            private static readonly MethodInfo CreateQueryMethod =
                ReflectionHelper.GetGenericMethodOf(_ => ((IQueryProvider)null).CreateQuery<int>(null));

            /// <summary>
            /// The generic method of IQueryable.Execute{TResult}.
            /// </summary>
            private static readonly MethodInfo ExecuteMethod =
                ReflectionHelper.GetGenericMethodOf(_ => ((IQueryProvider)null).Execute<int>(null));

            /// <summary>
            /// Gets or sets the provider.
            /// </summary>
            /// <value>
            /// The provider.
            /// </value>
            public SharePointQueryProvider Provider { get; set; }

            /// <summary>
            /// Constructs an <see cref="T:System.Linq.IQueryable"></see> object that can evaluate the query
            /// represented by a specified expression tree.
            /// </summary>
            /// <param name="expression">An expression tree that represents a LINQ query.</param>
            /// <returns>
            /// An <see cref="T:System.Linq.IQueryable"></see> that can evaluate the query represented by the
            /// specified expression tree.
            /// </returns>
            public IQueryable CreateQuery(Expression expression)
            {
                Requires.NotNull(expression, nameof(expression));

                var elementType = expression.Type.TryGetEnumerableItemType();
                var createQuery = CreateQueryMethod.MakeGenericMethod(elementType);
                return (IQueryable)createQuery.Call(this, expression);
            }

            /// <summary>
            /// Constructs an <see cref="T:System.Linq.IQueryable`1"></see> object that can evaluate the
            /// query represented by a specified expression tree.
            /// </summary>
            /// <typeparam name="TElement">The type of the elements of the
            ///                            <see cref="T:System.Linq.IQueryable`1"></see> that is returned.</typeparam>
            /// <param name="expression">An expression tree that represents a LINQ query.</param>
            /// <returns>
            /// An <see cref="T:System.Linq.IQueryable`1"></see> that can evaluate the query represented by
            /// the specified expression tree.
            /// </returns>
            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                return new SharePointQuery<TElement>(this.Provider, expression);
            }

            /// <summary>
            /// Executes the query represented by a specified expression tree.
            /// </summary>
            /// <param name="expression">An expression tree that represents a LINQ query.</param>
            /// <returns>
            /// The value that results from executing the specified query.
            /// </returns>
            public object Execute(Expression expression)
            {
                Requires.NotNull(expression, nameof(expression));

                var expressionType = expression.Type;
                var expressionElementType = expressionType.TryGetEnumerableItemType();
                if (expressionElementType != null)
                {
                    expressionType = typeof(IEnumerable<>).MakeGenericType(expressionElementType);
                }

                var execute = ExecuteMethod.MakeGenericMethod(expressionType);
                return execute.Call(this, expression);
            }

            public TResult Execute<TResult>(Expression expression)
            {
                var listFullName = this.Provider.QueryOperationContext.ListFullName();

                Requires.NotNull(listFullName, nameof(listFullName));

                var list = this.Provider.SiteService.GetListAsync(listFullName).GetResultNonLocking();
                var query = CamlQuery.CreateAllItemsQuery();
                var listItems = this.Provider.SiteService.GetListItemsAsync(list, query).GetResultNonLocking();
                var entities = listItems.Select(item => new SharePointEntity(this.Provider.ListTypeInfo, item)).ToList();

                return (TResult)(object)entities;
            }
        }
    }
}
