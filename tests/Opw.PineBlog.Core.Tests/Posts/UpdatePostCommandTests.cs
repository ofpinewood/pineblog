using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Opw.HttpExceptions;
using Opw.PineBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Posts
{
    public class UpdatePostCommandTests : MediatRTestsBase
    {
        private Guid _postId = Guid.NewGuid();

        public UpdatePostCommandTests()
        {
            var posts = new List<Post>();
            posts.Add(new Post
            {
                Id = _postId,
                AuthorId = Guid.NewGuid(),
                Title = "Post title 0",
                Slug = "post-title-0",
                Description = "Description",
                Content = "Content",
                Published = DateTime.UtcNow
            });

            PostRepositoryMock.Setup(m => m.SingleOrDefaultAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(posts.SingleOrDefault());
        }

        [Fact]
        public async Task Validator_Should_ThrowValidationErrorException()
        {
            Task action() => Mediator.Send(new UpdatePostCommand());

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(UpdatePostCommand.Id))).Should().NotBeNull();
            ex.Errors.Single(e => e.Key.Equals(nameof(UpdatePostCommand.Title))).Should().NotBeNull();
            ex.Errors.Single(e => e.Key.Equals(nameof(UpdatePostCommand.Content))).Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnNotFoundException()
        {
            PostRepositoryMock.Setup(m => m.SingleOrDefaultAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(Post));

            var result = await Mediator.Send(new UpdatePostCommand
            {
                Id = Guid.NewGuid(),
                Categories = "category",
                Title = "title",
                Content = "content",
                Description = "description"
            });

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<NotFoundException<Post>>();
        }

        [Fact]
        public async Task Handler_Should_UpdatePost()
        {
            var result = await Mediator.Send(new UpdatePostCommand
            {
                Id = _postId,
                Categories = "category",
                Title = "title-UPDATED",
                Content = "content",
                Description = "description"
            });

            result.IsSuccess.Should().BeTrue();
            result.Should().NotBeNull();
            result.Value.Title.Should().Be("title-UPDATED");
        }

        [Fact]
        public async Task Handler_Should_CoverUrl_ReplaceBaseUrlWithUrlFormat()
        {
            var result = await Mediator.Send(new UpdatePostCommand
            {
                Id = _postId,
                Categories = "category",
                Title = "title",
                Content = "content",
                Description = "description",
                CoverUrl = "http://127.0.0.1:10000/devstoreaccount1/pineblog-tests/blog-cover-url"
            });

            result.IsSuccess.Should().BeTrue();

            result.Value.Should().NotBeNull();
            result.Value.CoverUrl.Should().Be("%URL%/blog-cover-url");
        }

        [Fact]
        public async Task Handler_Should_UrlsInContent_ReplaceBaseUrlWithUrlFormat()
        {
            var result = await Mediator.Send(new UpdatePostCommand
            {
                Id = _postId,
                Categories = "category",
                Title = "title",
                Content = "content with an url: http://127.0.0.1:10000/devstoreaccount1/pineblog-tests/content-url-1. nice isn't it? And one more: http://127.0.0.1:10000/devstoreaccount1/pineblog-tests/content-url-2!",
                Description = "description",
            });

            result.IsSuccess.Should().BeTrue();

            result.Value.Should().NotBeNull();
            result.Value.Content.Should().Be("content with an url: %URL%/content-url-1. nice isn't it? And one more: %URL%/content-url-2!");
        }

        [Fact]
        public async Task Handler_Should_HaveCorrextSlug()
        {
            var result = await Mediator.Send(new UpdatePostCommand
            {
                Id = _postId,
                Categories = "category",
                Title = "title or slug",
                Content = "content",
                Description = "description"
            });

            result.IsSuccess.Should().BeTrue();
            result.Value.Title.Should().Be("title or slug");
            result.Value.Slug.Should().MatchRegex(result.Value.Title.ToPostSlug());
        }

        [Fact]
        public async Task Handler_Should_ReturnExceptionResult_WhenSaveChangesError()
        {
            BlogUnitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Result<int>.Fail(new ApplicationException("Error: SaveChangesAsync")));

            var result = await Mediator.Send(new UpdatePostCommand
            {
                Id = _postId,
                Categories = "category",
                Title = "title",
                Content = "content",
                Description = "description"
            });

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<ApplicationException>();
        }
    }
}
