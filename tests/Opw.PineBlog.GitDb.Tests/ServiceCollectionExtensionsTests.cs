using System;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Opw.PineBlog.FeatureManagement;
using Microsoft.Extensions.Hosting;

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
            var expectedMessage = "Disabled when using the GitDb data provider.  Please use the [repository](https://github.com/ofpinewood/pineblog-gitdb.git) to edit.";

            var featureManager = ServiceProvider.GetService<IFeatureManager>();

            featureManager.IsEnabled(FeatureFlag.AdminBlogSettings).IsEnabled.Should().BeFalse();
            featureManager.IsEnabled(FeatureFlag.AdminBlogSettings).Message.Should().Be(expectedMessage);
            featureManager.IsEnabled(FeatureFlag.AdminPosts).IsEnabled.Should().BeFalse();
            featureManager.IsEnabled(FeatureFlag.AdminPosts).Message.Should().Be(expectedMessage);
        }

        [Fact]
        public void AddPineBlogGitDb_Should_RegisterGitDbSyncService()
        {
            var hostedService = ServiceProvider.GetService<IHostedService>();

            hostedService.Should().NotBeNull();
            hostedService.Should().BeOfType<GitDbSyncService>();
        }
    }
}
