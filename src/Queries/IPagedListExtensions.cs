using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Strada.Framework.Queries
{
    /// <summary>
    /// Provides some extension methods for <see cref="IEnumerable{T}"/> to provide paging capability.
    /// </summary>
    public static class IPagedListExtensions
    {
        /// <summary>
        /// Converts to pagedlistasync.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="indexFrom">The index from.</param>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IEnumerable<T> source, int pageIndex, int pageSize, int indexFrom = 0)
            => new PagedList<T>(source, pageIndex, pageSize, indexFrom);

        /// <summary>
        /// Converts the specified source to <see cref="IPagedList{T}"/> by the specified <paramref name="converter"/>, <paramref name="pageIndex"/> and <paramref name="pageSize"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="source">The source to convert.</param>
        /// <param name="converter">The converter to change the <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="indexFrom">The start index value.</param>
        /// <returns>An instance of the inherited from <see cref="IPagedList{T}"/> interface.</returns>
        public static IPagedList<TResult> ToPagedList<TSource, TResult>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter, int pageIndex, int pageSize, int totalCount, int indexFrom = 0)
            => new PagedList<TSource, TResult>(source, converter, pageIndex, pageSize, totalCount, indexFrom);
    }
}
