using Microsoft.EntityFrameworkCore;
using Strada.Framework.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Strada.Framework.Persistence
{
    /// <summary>
    /// Generic repository implementation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ReadOnlyRepository{T}" />
    /// <seealso cref="IRepository{T}" />
    public class Repository<T> : ReadOnlyRepository<T>, IRepository<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public Repository(IContext context) : base(context)
        {
        }

        /// <summary>
        /// Add or update an entity in the context.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual async Task AddOrUpdateAsync(T entity, CancellationToken cancellationToken)
        {
            if (IsItNew(entity))
                await _dbContext.AddAsync(entity, cancellationToken);
            else
                await Task.Run(() => _dbContext.Update(entity));
        }

        /// <summary>
        /// Add or update an entity in the context.
        /// </summary>
        /// <param name="entities">Thes entities to add or update.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public virtual async Task AddOrUpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            foreach (var entity in entities.ToList())
                await AddOrUpdateAsync(entity, cancellationToken);
        }

        /// <summary>
        /// Add an entity in the context.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _dbContext.AddAsync(entity, cancellationToken);
        }

        /// <summary>
        /// Add a collection of entities in the context.
        /// </summary>
        /// <param name="entities">The entities to add or update.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual async Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            await _dbContext.AddRangeAsync(entities, cancellationToken);
        }

        /// <summary>
        /// Update an entity in the context.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            await Task.Run(() => _dbContext.Update(entity), cancellationToken);
        }

        /// <summary>
        /// Update a collection of entities in the context.
        /// </summary>
        /// <param name="entities">The entities to add or update.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual async Task UpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            await Task.Run(() => _dbContext.UpdateRange(entities), cancellationToken);
        }

        /// <summary>
        /// Removes the entity from the context.
        /// </summary>
        /// <param name="keyValues">The key values.</param>
        public virtual void RemoveByKey(params object[] keyValues)
        {
            var entity = _dbContext.Set<T>().Find(keyValues);
            if (entity != null)
            {
                Remove(entity);
            }
        }

        /// <summary>
        /// Removes the specified entity from the context.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        public virtual void Remove(T entity)
        {
            if (entity != null)
            {
                _dbContext.Set<T>().Attach(entity);
                _dbContext.Set<T>().Remove(entity);
            }
        }

        /// <summary>
        /// Removes the specified entity from the context.
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Remove(IEnumerable<T> entities)
        {
            entities?.ToList().ForEach((entity) => { Remove(entity); });
        }

        #region Private

        /// <summary>
        /// Determines whether the specified entity is new.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///   <c>true</c> if is it new otherwise, <c>false</c>.
        /// </returns>
        private bool IsItNew(object entity)
        {
            return !_dbContext.Entry(entity).IsKeySet;
        }

        #endregion
    }
}
