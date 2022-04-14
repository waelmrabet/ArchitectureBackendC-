using Microsoft.EntityFrameworkCore;
using Strada.Framework.Core;

namespace Persistence.UnitTests.Assets
{
    public interface ITestDbContext : IContext
    {
        DbSet<Customer> Customers { get; set; }
    }
}
