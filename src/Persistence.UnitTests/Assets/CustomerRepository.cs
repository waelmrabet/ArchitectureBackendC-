using Strada.Framework.Persistence;

namespace Persistence.UnitTests.Assets
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public CustomerRepository(ITestDbContext context)
            : base(context)
        {

        }
    }
}
