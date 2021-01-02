using FluentAssertions;
using FluentValidation.Results;
using Opw.PineBlog.Entities;
using Opw.HttpExceptions;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using Moq;
using System.Linq.Expressions;
using System.Threading;

namespace Opw.PineBlog.Posts
{
    public class GetPostByIdQueryTests : MediatRTestsBase
    {
        private Guid _postId = Guid.NewGuid();

        public GetPostByIdQueryTests()
        {
            var posts = new List<Post>();
            posts.Add(new Post
            {
                Id = _postId,
                AuthorId = Guid.NewGuid(),
                Title = "Post title 0",
                Slug = "post-title-0",
                Description = "Description",
                Content = "content with an url: %URL%/pineblog-tests/content-url-1. nice isn't it?",
                CoverUrl = "%URL%/pineblog-tests/blog-cover-url",
                Published = DateTime.UtcNow
            });

            PostRepositoryMock.Setup(m => m.SingleOrDefaultAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(posts.SingleOrDefault());

            AddBlogUnitOfWorkMock();
        }

        [Fact]
        public async Task Validator_Should_ThrowValidationErrorException()
        {
            Task action() => Mediator.Send(new GetPostByIdQuery());

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(GetPostByIdQuery.Id))).Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnNotFoundException()
        {
            PostRepositoryMock.Setup(m => m.SingleOrDefaultAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(Post));

            AddBlogUnitOfWorkMock();

            var result = await Mediator.Send(new GetPostByIdQuery { Id = Guid.NewGuid() });

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<NotFoundException<Post>>();
        }

        [Fact]
        public async Task Handler_Should_ReturnPost()
        {
            var result = await Mediator.Send(new GetPostByIdQuery { Id = _postId });

            result.IsSuccess.Should().BeTrue();
            result.Value.Title.Should().Be("Post title 0");
        }

        [Fact]
        public async Task Handler_Should_CoverUrl_ReplaceBaseUrlWithUrlFormat()
        {
            var result = await Mediator.Send(new GetPostByIdQuery { Id = _postId });

            result.IsSuccess.Should().BeTrue();

            result.Value.Should().NotBeNull();
            result.Value.CoverUrl.Should().Be("http://127.0.0.1:10000/devstoreaccount1/pineblog-tests/pineblog-tests/blog-cover-url");
        }

        [Fact]
        public async Task Handler_Should_UrlsInContent_ReplaceBaseUrlWithUrlFormat()
        {
            var result = await Mediator.Send(new GetPostByIdQuery { Id = _postId });

            result.IsSuccess.Should().BeTrue();

            result.Value.Should().NotBeNull();
            result.Value.Content.Should().Be("content with an url: http://127.0.0.1:10000/devstoreaccount1/pineblog-tests/pineblog-tests/content-url-1. nice isn't it?");
        }
    }
}
