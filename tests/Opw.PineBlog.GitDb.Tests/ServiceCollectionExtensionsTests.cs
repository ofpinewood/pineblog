using System;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Opw.PineBlog.FeatureManagement;

namespace Opw.PineBlog.GitDb
{
    public class ServiceCollectionExtensionsTests : GitDbTestsBase
    {
        public ServiceCollectionExtensionsTests(GitDbFixture fixture) : base(fixture)
        { }

        [Fact]
        public void AddPineBlogGitDb_Should_RegisterPineBlogGitDbOptions()
        {
            var options = ServiceProvider.GetService<IOptions<PineBlogGitDbOptions>>();

            options.Should().NotBeNull();
        }

        [Fact]
        public void AddPineBlogGitDb_Should_RegisterBlogUnitOfWork()
        {
            var uow = ServiceProvider.GetService<IBlogUnitOfWork>();

            uow.Should().NotBeNull();
        }

        [Fact]
        public void AddPineBlogGitDb_Should_ThrowConfigurationException_WhenConfigurationProviderNotConfigured()
        {
            var configuration = new ConfigurationBuilder()
               .Build();

            var services = new ServiceCollection();
            services.AddPineBlogCore(configuration);

            try
            {
                services.AddPineBlogGitDb(configuration);
            }
            catch (Exception ex)
            {
                ex.Should().BeOfType<ConfigurationException>();
                ex.Message.Should().Contain("The PineBlog IConfigurationProvider(s) are not configured");
            }
        }

        [Fact]
        public void AddPineBlogGitDb_Should_AddFeatureManagement()
        {
            var featureManager = ServiceProvider.GetService<IFeatureManager>();

            featureManager.IsEnabled(FeatureFlag.AdminBlogSettings).IsEnabled.Should().BeFalse();
            featureManager.IsEnabled(FeatureFlag.AdminBlogSettings).Message.Should().Be("Disabled when using GitDb as a data provider.");
            featureManager.IsEnabled(FeatureFlag.AdminPosts).IsEnabled.Should().BeFalse();
            featureManager.IsEnabled(FeatureFlag.AdminPosts).Message.Should().Be("Disabled when using GitDb as a data provider.");
        }
    }
}
