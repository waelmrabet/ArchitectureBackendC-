using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Strada.Framework.Core;
using Strada.Framework.Core.Security;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Strada.Framework.Persistence
{
    /// <summary>
    /// DbContext base class
    /// </summary>
    /// <seealso cref="DbContext" />
    /// <seealso cref="IContext" />
    public class DbContextBase : DbContext, IContext
    {
        private readonly ILogger<DbContextBase> _loggerService;
        private readonly IUserService _userService;

        /// <summary>
        /// Gets the context identifier.
        /// </summary>
        /// <value>
        /// The context identifier.
        /// </value>
        public Guid? Id
        {
            get
            {
                return _id ?? (_id = Guid.NewGuid());
            }
        }
        private Guid? _id;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextBase"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="loggerService">The logger service.</param>
        public DbContextBase(DbContextOptions options, ILogger<DbContextBase> loggerService)
            : base(options)
        {
            _loggerService = loggerService;

            // Default settings
            ChangeTracker.LazyLoadingEnabled = false;

            _loggerService.LogDebug("DbContext created");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextBase"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="loggerService">The logger service.</param>
        /// <param name="userService">The user service.</param>
        public DbContextBase(DbContextOptions options, ILogger<DbContextBase> loggerService, IUserService userService)
            : base(options)
        {
            _loggerService = loggerService;
            _userService = userService;

            // Default settings
            ChangeTracker.LazyLoadingEnabled = false;

            _loggerService.LogDebug("DbContext created");
        }

        /// <summary>
        /// <para>
        /// Saves all changes made in this context to the database.
        /// </para>
        /// <para>
        /// This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" /> to discover any
        /// changes to entity instances before saving to the underlying database. This can be disabled via
        /// <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        /// </para>
        /// <para>
        /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        /// that any asynchronous operations have completed before calling another method on this context.
        /// </para>
        /// </summary>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous save operation. The task result contains the
        /// number of state entries written to the database.
        /// </returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _loggerService.LogDebug("DbContext SaveChanges");

            int result;
            OnValidate();
            OnBeforeSaveChanges();
            result = await base.SaveChangesAsync(cancellationToken);
            OnAfterSaveChanges();

            return result;
        }

        /// <summary>
        /// Befores the SaveChanges.
        /// </summary>
        protected virtual void OnBeforeSaveChanges()
        {
        }

        /// <summary>
        /// After the SaveChanges.
        /// </summary>
        protected virtual void OnAfterSaveChanges()
        {
        }

        /// <summary>
        /// Validation before the SaveChanges
        /// </summary>
        protected virtual void OnValidate()
        {
            var entities = from e in ChangeTracker.Entries()
                           where e.State == EntityState.Added || e.State == EntityState.Modified
                           select e.Entity;

            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(entity, validationContext, validateAllProperties: true);
            }
        }

        /// <summary>
        /// <para>
        /// Override this method to configure the database (and other options) to be used for this context.
        /// This method is called for each instance of the context that is created.
        /// The base implementation does nothing.
        /// </para>
        /// <para>
        /// In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" /> may or may not have been passed
        /// to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" /> to determine if
        /// the options have already been set, and skip some or all of the logic in
        /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />.
        /// </para>
        /// </summary>
        /// <param name="optionsBuilder">A builder used to create or modify options for this context. Databases (and other extensions)
        /// typically define extension methods on this object that allow you to configure the context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging(true); // To show sql queries parameters values 
        }

        /// <summary>
        /// On model creating
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add soft delete to all entities that's implement ISoftDelete
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSoftDeleteQueryFilter();
                }
            }
        }

        /// <summary>
        /// Uses the auditable behaviour.
        /// </summary>
        protected virtual void UseAuditable()
        {
            string userId = _userService?.GetSubjectId();
            string userName = _userService?.GetName();

            // Change Created date & Modified date
            foreach (var entry in ChangeTracker.Entries<IAuditable>())
            {
                if (entry.Entity is IAuditable entity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        if (!string.IsNullOrEmpty(userName))
                        {
                            entity.CreatedBy = userName;
                        }

                        if (!string.IsNullOrEmpty(userId))
                        {
                            entity.CreatedById = userId;
                        }

                        entity.CreatedDate = DateTimeOffset.Now; // _dateTimeService.Now;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        if (!string.IsNullOrEmpty(userName))
                        {
                            entity.ModifiedBy = userName;
                        }

                        if (!string.IsNullOrEmpty(userId))
                        {
                            entity.ModifiedById = userId;
                        }

                        entity.ModifiedDate = DateTimeOffset.Now; //_dateTimeService.Now;
                    }
                }
            }
        }

        /// <summary>
        /// Uses the soft delete behaviour.
        /// </summary>
        protected virtual void UseSoftDelete()
        {
            foreach (var entry in ChangeTracker.Entries<ISoftDelete>())
            {
                if (entry.Entity is ISoftDelete softDelete && entry.State == EntityState.Deleted)
                {
                    softDelete.IsDeleted = true;
                    softDelete.DeletedDate = DateTimeOffset.Now; //_dateTimeService.Now;
                    entry.State = EntityState.Modified;
                }
            }
        }
    }
}
