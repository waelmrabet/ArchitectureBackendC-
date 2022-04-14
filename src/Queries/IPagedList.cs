using System.Collections.Generic;

namespace Strada.Framework.Queries
{
    /// <summary>
    /// Provides the interface for paged list of any type.
    /// </summary>
    /// <typeparam name="T">The type for paging</typeparam>
    public interface IPagedList<T>
    {
        /// <summary>
        /// Gets the index start value.
        /// </summary>
        int IndexFrom { get; }

        /// <summary>
        /// Gets the current page index.
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// Gets the page size.
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Gets the total count of the item list <typeparamref name="T"/>.
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// Gets the total pages.
        /// </summary>
        int TotalPages { get; }

        /// <summary>
        /// Gets the current pages items.
        /// </summary>
        IEnumerable<T> Items { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has previous page.
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has next page.
        /// </summary>
        bool HasNextPage { get; }
    }
}
