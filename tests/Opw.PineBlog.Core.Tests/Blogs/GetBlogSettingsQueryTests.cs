using FluentAssertions;
using Moq;
using Opw.PineBlog.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Blogs
{
    public class GetBlogSettingsQueryTests : MediatRTestsBase
    {
        public GetBlogSettingsQueryTests()
        {
            var blogSettings = new List<BlogSettings>();
            blogSettings.Add(new BlogSettings
            {
                Title = "blog title",
                Description = "blog description",
                CoverCaption = "blog cover caption",
                CoverLink = "blog cover link",
                CoverUrl = "%URL%/blog-cover-url"
            });

            BlogSettingsRepositoryMock.Setup(m => m.SingleOrDefaultAsync(It.IsAny<CancellationToken>())).ReturnsAsync(blogSettings.SingleOrDefault());

            AddBlogUnitOfWorkMock();
        }

        [Fact]
        public async Task Handler_Should_ReturnSettingsFromConfig_WhenNotFound()
        {
            BlogSettingsRepositoryMock.Setup(m => m.SingleOrDefaultAsync(It.IsAny<CancellationToken>())).ReturnsAsync(default(BlogSettings));
            AddBlogUnitOfWorkMock();

            var result = await Mediator.Send(new GetBlogSettingsQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Title.Should().Be("Title from configuration");
        }

        [Fact]
        public async Task Handler_Should_ReturnBlogSettings()
        {
            var result = await Mediator.Send(new GetBlogSettingsQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Title.Should().Be("blog title");
        }

        [Fact]
        public async Task Handler_Should_CoverUrl_ReplaceUrlFormatWithBaseUrl()
        {
            var result = await Mediator.Send(new GetBlogSettingsQuery());

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.CoverUrl.Should().Be("http://127.0.0.1:10000/devstoreaccount1/pineblog-tests/blog-cover-url");
        }
    }
}
