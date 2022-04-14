using Microsoft.Extensions.DependencyInjection;

namespace Persistence.UnitTests.Assets
{
    public static class ContextExtension
    {
        public static void ConfigureContextServices(this IServiceCollection services, ITestDbContext context)
        {
            // Read repository

            // Create / Update repository
            services.AddSingleton<ICustomerRepository>(new CustomerRepository(context));
        }
    }
}
