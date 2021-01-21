using Xunit;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;

namespace Opw.PineBlog.MongoDb
{
    public class ServiceCollectionExtensionsTests : MongoDbTestsBase
    {
        public ServiceCollectionExtensionsTests(MongoDbDatabaseFixture fixture) : base(fixture) { }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public void AddPineBlogMongoDb_Should_RegisterBlogUnitOfWork()
        {
            var uow = ServiceProvider.GetService<IBlogUnitOfWork>();

            uow.Should().NotBeNull();
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public void AddPineBlogMongoDb_Should_ThrowConfigurationException_WhenConfigurationProviderNotConfigured()
        {
            var configuration = new ConfigurationBuilder()
               .Build();

            var services = new ServiceCollection();
            services.AddPineBlogCore(configuration);

            try
            {
                services.AddPineBlogMongoDb(configuration);
            }
            catch (Exception ex)
            {
                ex.Should().BeOfType<ConfigurationException>();
                ex.Message.Should().Contain("The PineBlog IConfigurationProvider(s) are not configured");
            }
        }
    }
}
