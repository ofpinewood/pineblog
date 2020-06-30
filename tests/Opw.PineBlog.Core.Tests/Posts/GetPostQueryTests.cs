using FluentAssertions;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.Entities;
using Opw.HttpExceptions;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Opw.PineBlog.EntityFrameworkCore;

namespace Opw.PineBlog.Posts
{
    public class GetPostQueryTests : MediatRTestsBase
    {
        public GetPostQueryTests()
        {
            SeedDatabase();
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
            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-invalid" });

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<NotFoundException<Post>>();
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithFirstPost()
        {
            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-0" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Title.Should().Be("Post title 0");
            result.Value.Previous.Should().BeNull();
            result.Value.Next.Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithMiddlePost()
        {
            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-2" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Title.Should().Be("Post title 2");
            result.Value.Previous.Should().NotBeNull();
            result.Value.Next.Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithMiddlePost_WithCorrectPreviousPost()
        {
            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-2" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Title.Should().Be("Post title 2");
            result.Value.Previous.Title.Should().Be("Post title 1");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithMiddlePost_WithCorrectNextPost()
        {
            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-2" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Title.Should().Be("Post title 2");
            result.Value.Next.Title.Should().Be("Post title 3");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithLastPost()
        {
            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-4" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Title.Should().Be("Post title 4");
            result.Value.Previous.Should().NotBeNull();
            result.Value.Next.Should().BeNull();
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
            result.Value.Post.CoverUrl.Should().Be("https://ofpinewood.com/cover-url");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithPostAuthor()
        {
            var result = await Mediator.Send(new GetPostQuery { Slug = "post-title-0" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Should().NotBeNull();
            result.Value.Post.Author.DisplayName.Should().Be("Author 1");
        }

        private void SeedDatabase()
        {
            var context = ServiceProvider.GetRequiredService<BlogEntityDbContext>();

            var author = new Author { UserName = "user@example.com", DisplayName = "Author 1" };
            context.Authors.Add(author);
            context.SaveChanges();

            context.Posts.Add(CreatePost(0, author.Id));
            context.Posts.Add(CreatePost(1, author.Id));
            context.Posts.Add(CreatePost(2, author.Id));
            context.Posts.Add(CreatePost(3, author.Id));
            context.Posts.Add(CreatePost(4, author.Id));
            context.SaveChanges();
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
