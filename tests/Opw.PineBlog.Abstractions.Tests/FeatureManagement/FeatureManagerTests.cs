using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.FeatureManagement
{
    public class FeatureManagerTests
    {
        private readonly FeatureManager _featureManager;

        public FeatureManagerTests()
        {
            var features = new Dictionary<FeatureFlag, FeatureState>();
            features.Add(FeatureFlag.AdminBlogSettings, FeatureState.Enabled());
            features.Add(FeatureFlag.AdminPosts, FeatureState.Disabled("Disabled!"));

            _featureManager = new FeatureManager(features);
        }

        [Fact]
        public void IsEnabled_Should_BeTrue_ForAdminBlogSettings()
        {
            var result = _featureManager.IsEnabled(FeatureFlag.AdminBlogSettings);

            result.IsEnabled.Should().BeTrue();
        }

        [Fact]
        public void IsEnabled_Should_BeFalse_ForAdminPosts()
        {
            var result = _featureManager.IsEnabled(FeatureFlag.AdminPosts);

            result.IsEnabled.Should().BeFalse();
            result.Message.Should().Be("Disabled!");
        }

        [Fact]
        public async Task IsEnabledAsync_Should_BeTrue_ForAdminBlogSettings()
        {
            var result = await _featureManager.IsEnabledAsync(FeatureFlag.AdminBlogSettings);

            result.IsEnabled.Should().BeTrue();
        }
    }
}
