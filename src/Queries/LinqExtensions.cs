using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Strada.Framework.Queries
{
    /// <summary>
    /// Linq Extensions
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Extension method to order a dataset by properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="propertiesName">List of the properties name.</param>
        /// <param name="comparer">The comparer.</param>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string[] propertiesName, IComparer<object> comparer = null)
        {
            IOrderedQueryable<T> result = null;

            for(int i = 0; i < propertiesName.Length; i++)
            {
                if (i == 0)
                    result = CallOrderedQueryable(query, "OrderBy", propertiesName[i], comparer);
                else
                    result = CallOrderedQueryable(query, "ThenBy", propertiesName[i], comparer);
            }
            return result;
        }

        /// <summary>
        /// Extension method to order a dataset by properties descending.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="propertiesName">List of the properties name.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string[] propertiesName, IComparer<object> comparer = null)
        {
            IOrderedQueryable<T> result = null;

            for(int i = 0; i < propertiesName.Length; i++)
            {
                if (i == 0)
                    result = CallOrderedQueryable(query, "OrderByDescending", propertiesName[i], comparer);
                else
                    result = CallOrderedQueryable(query, "ThenByDescending", propertiesName[i], comparer);
            }
            return result;
        }

        /// <summary>
        /// Create the order query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="methodName">Name of the method "OrderBy" or "ThenBy".</param>
        /// <param name="propertyName">Name of the property to order</param>
        /// <param name="comparer">The comparer.</param>
        public static IOrderedQueryable<T> CallOrderedQueryable<T>(this IQueryable<T> query, string methodName, string propertyName,
            IComparer<object> comparer = null)
        {
            var param = Expression.Parameter(typeof(T), "x");

            var body = propertyName.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);

            return comparer != null
                ? (IOrderedQueryable<T>)query.Provider.CreateQuery(
                    Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new[] { typeof(T), body.Type },
                        query.Expression,
                        Expression.Lambda(body, param),
                        Expression.Constant(comparer)
                        )
                    )
                : (IOrderedQueryable<T>)query.Provider.CreateQuery(
                    Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new[] { typeof(T), body.Type },
                        query.Expression,
                        Expression.Lambda(body, param)
                        )
                    );
        }

        /// <summary>
        /// Extension method to order the dataset.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="sortField">The sort field.</param>
        /// <param name="sortOrder">The sort order. ASC / DESC</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> OrderByFilter<TSource>(this IQueryable<TSource> query, string[] sortField, Order sortOrder, IComparer<object> comparer = null)
        {
            if (sortOrder == Order.ASC)
                return OrderBy(query, sortField, comparer);
            else
                return OrderByDescending(query, sortField, comparer);
        }
    }
}
