using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Strada.Framework.Core.Security;
using Strada.Framework.Persistence;

namespace Persistence.UnitTests.Assets
{
    /// <summary>
    /// TestContext
    /// </summary>
    /// <seealso cref="Strada.Framework.Persistence.DbContextBase" />
    public class TestDbContext : DbContextBase, ITestDbContext
    {
        /// <summary>
        /// Gets or sets the customers.
        /// </summary>
        /// <value>
        /// The customers.
        /// </value>
        public DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="loggerService">The logger service.</param>
        /// <param name="userService">The user service.</param>
        public TestDbContext(DbContextOptions options, ILogger<DbContextBase> loggerService, IUserService userService) : 
            base(options, loggerService, userService)
        {
        }

        /// <summary>
        /// Called before save changes.
        /// </summary>
        protected override void OnBeforeSaveChanges()
        {
            UseAuditable();
            UseSoftDelete();
            base.OnBeforeSaveChanges();
        }
    }
}
