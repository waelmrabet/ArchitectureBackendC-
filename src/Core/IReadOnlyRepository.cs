using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Strada.Framework.Core
{
    /// <summary>
    /// Generic read only repository interface. 
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IReadOnlyRepository<T> : IDisposable
    {
        /// <summary>
        /// Get all the entities of type T.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A list of T
        /// </returns>
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Finds an entity with the specified key values.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="keyValues">The key values.</param>
        /// <returns>
        /// Return the entity or null
        /// </returns>
        Task<T> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
    }
}
