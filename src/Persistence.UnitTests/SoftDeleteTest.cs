using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Persistence.UnitTests.Assets
{
    [TestClass]
    public class SoftDeleteTest
    {
        [Fact]
        public async Task Test_Soft_Delete()
        {
            // Arange 
            Guid guid = new Guid("f3d6e954-3736-441c-983f-cb8b55871972");
            var context = InMemoryContext.GetInMemoryContext();
            context.Database.EnsureCreated();

            var serviceCollection = new ServiceCollection();
            serviceCollection.ConfigureContextServices(context);
            
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var unitOfWork = new TestUnitOfWork(context, serviceProvider);

            await unitOfWork.CustomerRepository.AddAsync(new Customer { Id = guid, FirstName = "Victor", LastName = "Hugo" }, CancellationToken.None);
            await unitOfWork.SaveAsync();

            // Act
            unitOfWork.CustomerRepository.RemoveByKey(guid);
            await unitOfWork.SaveAsync();

            // Assert
            context.Customers.Count().Should<int>().Be(0);
            context.Customers.IgnoreQueryFilters().Count().Should<int>().Be(1);
        }              
    }
}
