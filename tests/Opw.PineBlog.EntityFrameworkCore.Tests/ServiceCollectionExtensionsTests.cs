using System;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace Opw.PineBlog.EntityFrameworkCore
{
    public class ServiceCollectionExtensionsTests : EntityFrameworkCoreTestsBase
    {
        [Fact]
        public void AddPineBlogEntityFrameworkCore_Should_RegisterBlogEntityDbContext()
        {
            var repo = ServiceProvider.GetService<IRepository>();

            repo.Should().NotBeNull();
        }

        [Fact]
        public void AddPineBlogEntityFrameworkCore_Should_ThrowConfigurationException_WhenConfigurationProviderNotConfigured()
        {
            var configuration = new ConfigurationBuilder()
               .Build();

            var services = new ServiceCollection();
            services.AddPineBlogCore(configuration);

            try
            {
                services.AddPineBlogEntityFrameworkCore(configuration);
            }
            catch (Exception ex)
            {
                ex.Should().BeOfType<ConfigurationException>();
                ex.Message.Should().Contain("The PineBlog IConfigurationProvider(s) are not configured");
            }
        }
    }
}
