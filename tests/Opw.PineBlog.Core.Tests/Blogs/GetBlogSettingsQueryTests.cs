using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.Entities;
using System.Threading.Tasks;
using Xunit;
using System.Threading;

namespace Opw.PineBlog.Blogs
{
    public class GetBlogSettingsQueryTests : MediatRTestsBase
    {
        public GetBlogSettingsQueryTests()
        {
        }

        [Fact]
        public async Task Handler_Should_ReturnSettingsFromConfig_WhenNotFound()
        {
            await SeedDatabase();

            var repo = ServiceProvider.GetRequiredService<IRepository>();

            var existing = await repo.GetBlogSettingsAsync(CancellationToken.None);
            await repo.DeleteBlogSettingsAsync(existing, CancellationToken.None);

            var result = await Mediator.Send(new GetBlogSettigsQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Title.Should().Be("Title from configuration");
        }

        [Fact]
        public async Task Handler_Should_ReturnBlogSettings()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetBlogSettigsQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Title.Should().Be("blog title");
        }

        private async Task SeedDatabase()
        {
            var repo = ServiceProvider.GetRequiredService<IRepository>();

            await repo.UpdateBlogSettingsAsync(new BlogSettings
            {
                Title = "blog title",
                Description = "blog description",
                CoverCaption = "blog cover caption",
                CoverLink = "blog cover link",
                CoverUrl = "blog cover url"
            }, CancellationToken.None);
        }
    }
}
