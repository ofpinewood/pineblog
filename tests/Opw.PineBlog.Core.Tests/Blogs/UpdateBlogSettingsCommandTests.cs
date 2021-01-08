using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Opw.HttpExceptions;
using Opw.PineBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Blogs
{
    public class UpdateBlogSettingsCommandTests : MediatRTestsBase
    {
        public UpdateBlogSettingsCommandTests()
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
        }

        [Fact]
        public async Task Validator_Should_ThrowValidationErrorException()
        {
            Task action() => Mediator.Send(new UpdateBlogSettingsCommand());

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(UpdateBlogSettingsCommand.Title))).Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_AddSettings_WhenNotFound()
        {
            BlogSettings resultBlogSettings = null;

            BlogSettingsRepositoryMock.Setup(m => m.SingleOrDefaultAsync(It.IsAny<CancellationToken>())).ReturnsAsync(default(BlogSettings));
            BlogSettingsRepositoryMock.Setup(m => m.Add(It.IsAny<BlogSettings>())).Callback((BlogSettings bs) => resultBlogSettings = bs);

            var result = await Mediator.Send(new UpdateBlogSettingsCommand
            {
                Title = "blog title-NEW"
            });

            result.IsSuccess.Should().BeTrue();

            BlogSettingsRepositoryMock.Verify(m => m.Add(It.IsAny<BlogSettings>()), Times.Once);
            BlogSettingsRepositoryMock.Verify(m => m.Update(It.IsAny<BlogSettings>()), Times.Never);

            resultBlogSettings.Should().NotBeNull();
            resultBlogSettings.Title.Should().Be("blog title-NEW");
        }

        [Fact]
        public async Task Handler_Should_CoverUrl_ReplaceBaseUrlWithUrlFormat()
        {
            BlogSettings resultBlogSettings = null;

            BlogSettingsRepositoryMock.Setup(m => m.SingleOrDefaultAsync(It.IsAny<CancellationToken>())).ReturnsAsync(default(BlogSettings));
            BlogSettingsRepositoryMock.Setup(m => m.Add(It.IsAny<BlogSettings>())).Callback((BlogSettings bs) => resultBlogSettings = bs);

            var result = await Mediator.Send(new UpdateBlogSettingsCommand
            {
                Title = "blog title-NEW",
                CoverUrl = "http://127.0.0.1:10000/devstoreaccount1/pineblog-tests/blog-cover-url"
            });

            result.IsSuccess.Should().BeTrue();

            resultBlogSettings.Should().NotBeNull();
            resultBlogSettings.CoverUrl.Should().Be("%URL%/blog-cover-url");
        }

        [Fact]
        public async Task Handler_Should_UpdateBlogSettings()
        {
            var existingBlogSettings = new BlogSettings
            {
                Title = "blog title-UPDATED",
                Description = "blog description-UPDATED",
                CoverCaption = "blog cover caption-UPDATED",
                CoverLink = "blog cover link-UPDATED",
                CoverUrl = "blog cover url-UPDATED",
                Created = DateTime.UtcNow,
            };

            BlogSettings resultBlogSettings = null;

            BlogSettingsRepositoryMock.Setup(m => m.SingleOrDefaultAsync(It.IsAny<CancellationToken>())).ReturnsAsync(existingBlogSettings);
            BlogSettingsRepositoryMock.Setup(m => m.Update(It.IsAny<BlogSettings>())).Callback((BlogSettings bs) => resultBlogSettings = bs);

            var result = await Mediator.Send(new UpdateBlogSettingsCommand
            {
                Title = "blog title-UPDATED",
                Description = "blog description-UPDATED",
                CoverCaption = "blog cover caption-UPDATED",
                CoverLink = "blog cover link-UPDATED",
                CoverUrl = "blog cover url-UPDATED"
            });

            result.IsSuccess.Should().BeTrue();

            resultBlogSettings.Should().NotBeNull();
            resultBlogSettings.Title.Should().Be("blog title-UPDATED");
            resultBlogSettings.Description.Should().Be("blog description-UPDATED");
            resultBlogSettings.CoverCaption.Should().Be("blog cover caption-UPDATED");
            resultBlogSettings.CoverLink.Should().Be("blog cover link-UPDATED");
            resultBlogSettings.CoverUrl.Should().Be("blog cover url-UPDATED");
        }

        [Fact]
        public async Task Handler_Should_ReturnExceptionResult_WhenSaveChangesError()
        {
            BlogSettingsRepositoryMock.Setup(m => m.SingleOrDefaultAsync(It.IsAny<CancellationToken>())).ReturnsAsync(default(BlogSettings));

            BlogUnitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Result<int>.Fail(new ApplicationException("Error: SaveChangesAsync")));

            var result = await Mediator.Send(new UpdateBlogSettingsCommand
            {
                Title = "blog title-NEW",
            });

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<ApplicationException>();
        }
    }
}
