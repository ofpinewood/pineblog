using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.Entities;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Opw.PineBlog.EntityFrameworkCore;

namespace Opw.PineBlog.Blogs
{
    public class GetBlogSettingsQueryTests : MediatRTestsBase
    {
        public GetBlogSettingsQueryTests()
        {
            SeedDatabase();
        }

        [Fact]
        public async Task Handler_Should_ReturnSettingsFromConfig_WhenNotFound()
        {
            var context = ServiceProvider.GetRequiredService<BlogEntityDbContext>();

            var existing = await context.BlogSettings.SingleAsync();
            context.BlogSettings.Remove(existing);
            await context.SaveChangesAsync(true, default);

            var result = await Mediator.Send(new GetBlogSettigsQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Title.Should().Be("Title from configuration");
        }

        [Fact]
        public async Task Handler_Should_ReturnBlogSettings()
        {
            var result = await Mediator.Send(new GetBlogSettigsQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Title.Should().Be("blog title");
        }

        private void SeedDatabase()
        {
            var context = ServiceProvider.GetRequiredService<BlogEntityDbContext>();

            context.BlogSettings.Add(new BlogSettings
            {
                Title = "blog title",
                Description = "blog description",
                CoverCaption = "blog cover caption",
                CoverLink = "blog cover link",
                CoverUrl = "blog cover url"
            });
            context.SaveChanges();
        }
    }
}
