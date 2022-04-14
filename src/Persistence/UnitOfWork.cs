using Microsoft.EntityFrameworkCore;
using Npgsql.Bulk;
using Strada.Framework.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Strada.Framework.Persistence
{
    /// <summary>
    /// Implémentation de base du pattern unit of work
    /// </summary>
    /// <typeparam name="TContext">Type du context</typeparam>
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : IContext
    {
        /// <summary>
        /// Obtient l'dentifiant du context
        /// </summary>
        public Guid? ContextId
        {
            get
            {
                return _contextId ?? (_contextId = Guid.NewGuid());
            }
        }
        private Guid? _contextId;

        /// <summary>
        /// Obtient le current context de type <see cref="IContext"/>
        /// </summary>
        public IContext Context
        {
            get { return (TContext)_context; }
        }
        private readonly IContext _context;

        /// <summary>
        /// Constructeur avec un context à initialiser
        /// </summary>
        /// <param name="context">Context à initialiser</param>
        public UnitOfWork(IContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Commit tous les changements du context courant
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
           return await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Bulks the insert.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">The entities.</param>
        public async Task BulkInsertAsync<T>(IEnumerable<T> entities) where T : class
        {
            var uploader = new NpgsqlBulkUploader(Context as DbContext);
            await uploader.InsertAsync(entities);
        }

        /// <summary>
        /// Nettoyage des ressources utlisées
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Nettoyage des ressources utlisées
        /// </summary>
        /// <param name="disposing">True permet de libérer toutes les resources utilisées</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _context != null)
            {
                //_context.Dispose(); // Done by the container
            }
        }
    }
}
