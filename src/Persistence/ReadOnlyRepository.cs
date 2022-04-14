using Microsoft.EntityFrameworkCore;
using Strada.Framework.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Strada.Framework.Persistence
{
    /// <summary>
    /// Generic read only repository implementation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IReadOnlyRepository{T}" />
    public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
    {
        /// <summary>
        /// The _DB context
        /// </summary>
        protected readonly DbContextBase _dbContext;

        /// <summary>
        /// Gets the DbContext
        /// </summary>
        protected DbContextBase DbContext
        {
            get { return _dbContext; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ReadOnlyRepository(IContext context)
        {
            _dbContext = context as DbContextBase;
        }

        /// <summary>
        /// Get all the entities of type T.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A list of T
        /// </returns>
        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Finds an entity with the specified key values.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="keyValues">The key values.</param>
        /// <returns>
        /// Return the entity or null
        /// </returns>
        public virtual async Task<T> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await _dbContext.Set<T>().FindAsync(keyValues);
        }

        /// <summary>
        /// Nettoyage des ressources utilisées
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Nettoyage des ressources utilisées
        /// </summary>
        /// <param name="disposing">True permet libérer toutes les resources utilisées</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Clean 
            }
        }
    }
}
