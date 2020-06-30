using Xunit;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;

namespace Opw.PineBlog.MongoDb
{
    public class ServiceCollectionExtensionsTests
    {
        protected readonly IServiceCollection Services;

        protected IServiceProvider ServiceProvider => Services.BuildServiceProvider();

        public ServiceCollectionExtensionsTests()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddPineBlogMongoDbConfiguration(reloadOnChange: false)
               .Build();

            Services = new ServiceCollection();
            Services.AddPineBlogCore(configuration);
            Services.AddPineBlogMongoDb(configuration);
        }

        [Fact]
        public void AddPineBlogMongoDb_Should_RegisterBlogUnitOfWork()
        {
            var uow = ServiceProvider.GetService<IBlogUnitOfWork>();

            uow.Should().NotBeNull();
        }
    }
}
