using Microsoft.Extensions.DependencyInjection;
using Strada.Framework.Persistence;
using System;

namespace Persistence.UnitTests.Assets
{
    public class TestUnitOfWork : UnitOfWork<TestDbContext>, ITestUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;

        public ICustomerRepository CustomerRepository
        {
            get { return _customerRepository ?? (_customerRepository = _serviceProvider.GetRequiredService<ICustomerRepository>()); }
        }
        private ICustomerRepository _customerRepository;

        public TestUnitOfWork(ITestDbContext context, IServiceProvider serviceProvider)
            : base(context)
        {
            _serviceProvider = serviceProvider;
        }
    }
}
