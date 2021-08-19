using Xunit;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using Opw.PineBlog.FeatureManagement;

namespace Opw.PineBlog
{
    public class ServiceCollectionExtensionsTests : MediatRTestsBase
    {
        [Fact]
        public void AddPineBlogCore_Should_AddFeatureManagement()
        {
            var featureManager = ServiceProvider.GetService<IFeatureManager>();

            featureManager.IsEnabled(FeatureFlag.AdminBlogSettings).IsEnabled.Should().BeTrue();
            featureManager.IsEnabled(FeatureFlag.AdminPosts).IsEnabled.Should().BeTrue();
        }
    }
}
