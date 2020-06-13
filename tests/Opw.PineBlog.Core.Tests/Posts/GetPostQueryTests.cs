using FluentAssertions;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.Entities;
using Opw.HttpExceptions;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using System.Threading;

namespace Opw.PineBlog.Posts
{
    public class GetPostQueryTests : MediatRTestsBase
    {
        public GetPostQueryTests()
        {
        }

        [Fact]
        public async Task Validator_Should_ThrowValidationErrorException()
        {
            await SeedDatabase();

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
            await SeedDatabase();

            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-invalid" });

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<NotFoundException<Post>>();
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithFirstPost()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-0" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Title.Should().Be("Post title 0");
            result.Value.Previous.Should().BeNull();
            result.Value.Next.Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithMiddlePost()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-2" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Title.Should().Be("Post title 2");
            result.Value.Previous.Should().NotBeNull();
            result.Value.Next.Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithMiddlePost_WithCorrectPreviousPost()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-2" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Title.Should().Be("Post title 2");
            result.Value.Previous.Title.Should().Be("Post title 1");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithMiddlePost_WithCorrectNextPost()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-2" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Title.Should().Be("Post title 2");
            result.Value.Next.Title.Should().Be("Post title 3");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithLastPost()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-4" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Title.Should().Be("Post title 4");
            result.Value.Previous.Should().NotBeNull();
            result.Value.Next.Should().BeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithBlogInfo()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-0" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Should().NotBeNull();
            result.Value.Blog.Title.Should().Be("Title from configuration");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithPostCover()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-0" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Should().NotBeNull();
            result.Value.Post.CoverUrl.Should().Be("https://ofpinewood.com/cover-url");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithPostAuthor()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-0" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Should().NotBeNull();
            result.Value.Post.Author.DisplayName.Should().Be("Author 1");
        }

        private async Task SeedDatabase()
        {
            var repo = ServiceProvider.GetRequiredService<IRepository>();
            CancellationToken cancelToken = CancellationToken.None;

            var author = new Author { UserName = "user@example.com", DisplayName = "Author 1" };
            await repo.AddAuthorAsync(author, cancelToken);

            await repo.AddPostAsync(CreatePost(0, author.Id), cancelToken);
            await repo.AddPostAsync(CreatePost(1, author.Id), cancelToken);
            await repo.AddPostAsync(CreatePost(2, author.Id), cancelToken);
            await repo.AddPostAsync(CreatePost(3, author.Id), cancelToken);
            await repo.AddPostAsync(CreatePost(4, author.Id), cancelToken);
        }

        private Post CreatePost(int i, Guid authorId)
        {
            return new Post
            {
                AuthorId = authorId,
                Title = "Post title " + i,
                Slug = "post-title-" + i,
                Description = "Description",
                Content = "Content",
                Published = DateTime.UtcNow.AddDays(-30 + i),
                CoverUrl = "https://ofpinewood.com/cover-url",
                CoverCaption = "Cover caption",
                CoverLink = "https://ofpinewood.com/cover-link"
            };
        }
    }
}
