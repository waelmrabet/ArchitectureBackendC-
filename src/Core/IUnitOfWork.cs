using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Strada.Framework.Core
{
    /// <summary>
    /// Unit of work interface
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the context identifier.
        /// </summary>
        /// <value>
        /// The context identifier.
        /// </value>
        Guid? ContextId { get; }

        /// <summary>
        /// Saves the context asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<int> SaveAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// Bulks the insert asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        Task BulkInsertAsync<T>(IEnumerable<T> entities) where T : class;
    }
}
