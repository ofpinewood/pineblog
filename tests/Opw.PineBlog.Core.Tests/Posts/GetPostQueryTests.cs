using FluentAssertions;
using FluentValidation.Results;
using Opw.PineBlog.Entities;
using Opw.HttpExceptions;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Moq;
using System.Threading;
using System.Linq.Expressions;

namespace Opw.PineBlog.Posts
{
    public class GetPostQueryTests : MediatRTestsBase
    {
        private readonly Author _author;

        public GetPostQueryTests()
        {
            _author = new Author { UserName = "user@example.com", DisplayName = "Author 1" };

            PostRepositoryMock.Setup(m => m.SingleOrDefaultAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(CreatePost(0, _author));

            AddBlogUnitOfWorkMock();
        }

        [Fact]
        public async Task Validator_Should_ThrowValidationErrorException()
        {
            Task action() => Mediator.Send(new GetPostQuery
            {
                Slug = "this is not a valid slug"
            });

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(GetPostQuery.Slug))).Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnNotFoundException()
        {
            PostRepositoryMock.Setup(m => m.SingleOrDefaultAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(Post));

            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-invalid" });

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<NotFoundException<Post>>();
        }

        [Fact]
        public async Task Handler_Should_Predicates_PublishedNotNullAndSlugEqualsExpression()
        {
            Expression<Func<Post, bool>> createdPredicate = null;

            PostRepositoryMock
                .Setup(m => m.SingleOrDefaultAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<CancellationToken>()))
                .Callback((Expression<Func<Post, bool>> predicate, CancellationToken _) => createdPredicate = predicate)
                .ReturnsAsync(CreatePost(0, _author));

            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-invalid" });

            result.IsSuccess.Should().BeTrue();

            Expression<Func<Post, bool>> expression = p => p.Published != null && p.Slug.Equals("post-title-invalid");
            createdPredicate.Should().BeEquivalentTo(expression);
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithNoNextPost()
        {
            PostRepositoryMock.Setup(m => m.GetPreviousAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ReturnsAsync(CreatePost(2, _author));

            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-0" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Title.Should().Be("Post title 0");
            result.Value.Next.Should().BeNull();
            result.Value.Previous.Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithNoPreviousPost()
        {
            PostRepositoryMock.Setup(m => m.GetNextAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ReturnsAsync(CreatePost(2, _author));

            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-0" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Title.Should().Be("Post title 0");
            result.Value.Previous.Should().BeNull();
            result.Value.Next.Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithBlogInfo()
        {
            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-0" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Should().NotBeNull();
            result.Value.Blog.Title.Should().Be("Title from configuration");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithPostCover()
        {
            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-0" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Should().NotBeNull();
            result.Value.Post.CoverUrl.Should().Be("http://127.0.0.1:10000/devstoreaccount1/pineblog-tests/pineblog-tests/blog-cover-url");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithPostAuthor()
        {
            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-0" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Should().NotBeNull();
            result.Value.Post.Author.DisplayName.Should().Be("Author 1");
        }

        [Fact]
        public async Task Handler_Should_Post_CoverUrl_ReplaceBaseUrlWithUrlFormat()
        {
            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-0" });

            result.IsSuccess.Should().BeTrue();

            result.Value.Should().NotBeNull();
            result.Value.Post.CoverUrl.Should().Be("http://127.0.0.1:10000/devstoreaccount1/pineblog-tests/pineblog-tests/blog-cover-url");
        }

        [Fact]
        public async Task Handler_Should_Post_UrlsInContent_ReplaceBaseUrlWithUrlFormat()
        {
            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-0" });

            result.IsSuccess.Should().BeTrue();

            result.Value.Should().NotBeNull();
            result.Value.Post.Content.Should().Be("content with an url: http://127.0.0.1:10000/devstoreaccount1/pineblog-tests/pineblog-tests/content-url-1. nice isn't it?");
        }

        [Fact]
        public async Task Handler_Should_Next_CoverUrl_ReplaceBaseUrlWithUrlFormat()
        {
            PostRepositoryMock.Setup(m => m.GetNextAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ReturnsAsync(CreatePost(2, _author));

            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-0" });

            result.IsSuccess.Should().BeTrue();

            result.Value.Should().NotBeNull();
            result.Value.Next.CoverUrl.Should().Be("http://127.0.0.1:10000/devstoreaccount1/pineblog-tests/pineblog-tests/blog-cover-url");
        }

        [Fact]
        public async Task Handler_Should_Next_UrlsInContent_ReplaceBaseUrlWithUrlFormat()
        {
            PostRepositoryMock.Setup(m => m.GetNextAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ReturnsAsync(CreatePost(2, _author));

            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-0" });

            result.IsSuccess.Should().BeTrue();

            result.Value.Should().NotBeNull();
            result.Value.Next.Content.Should().Be("content with an url: http://127.0.0.1:10000/devstoreaccount1/pineblog-tests/pineblog-tests/content-url-1. nice isn't it?");
        }

        [Fact]
        public async Task Handler_Should_Previous_CoverUrl_ReplaceBaseUrlWithUrlFormat()
        {
            PostRepositoryMock.Setup(m => m.GetPreviousAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ReturnsAsync(CreatePost(2, _author));

            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-0" });

            result.IsSuccess.Should().BeTrue();

            result.Value.Should().NotBeNull();
            result.Value.Previous.CoverUrl.Should().Be("http://127.0.0.1:10000/devstoreaccount1/pineblog-tests/pineblog-tests/blog-cover-url");
        }

        [Fact]
        public async Task Handler_Should_Previous_UrlsInContent_ReplaceBaseUrlWithUrlFormat()
        {
            PostRepositoryMock.Setup(m => m.GetPreviousAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ReturnsAsync(CreatePost(2, _author));

            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-0" });

            result.IsSuccess.Should().BeTrue();

            result.Value.Should().NotBeNull();
            result.Value.Previous.Content.Should().Be("content with an url: http://127.0.0.1:10000/devstoreaccount1/pineblog-tests/pineblog-tests/content-url-1. nice isn't it?");
        }

        private Post CreatePost(int i, Author author)
        {
            return new Post
            {
                Author = author,
                Title = "Post title " + i,
                Slug = "post-title-" + i,
                Description = "Description",
                Content = "content with an url: %URL%/pineblog-tests/content-url-1. nice isn't it?",
                CoverUrl = "%URL%/pineblog-tests/blog-cover-url",
                Published = DateTime.UtcNow.AddDays(-30 + i),
                CoverCaption = "Cover caption",
                CoverLink = "https://ofpinewood.com/cover-link"
            };
        }
    }
}
