using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Strada.Framework.Core.Security;
using System;

namespace Persistence.UnitTests.Assets
{
    public static class InMemoryContext
    {
        public static TestDbContext GetInMemoryContext()
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            ILogger<TestDbContext> logger = loggerFactory.CreateLogger<TestDbContext>();
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>().UseInMemoryDatabase("UnitTests" + DateTime.Now.ToFileTimeUtc()).EnableSensitiveDataLogging();
            IUserService userService = new UserService();

            return new TestDbContext(optionsBuilder.Options, logger, userService);
        }
    }
}
