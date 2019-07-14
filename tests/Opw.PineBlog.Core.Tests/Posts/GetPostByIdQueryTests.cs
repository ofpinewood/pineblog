using FluentAssertions;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.Entities;
using Opw.HttpExceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Posts
{
    public class GetPostByIdQueryTests : MediatRTestsBase
    {
        protected Guid PostId { get; set; }

        public GetPostByIdQueryTests()
        {
            SeedDatabase();
        }

        [Fact]
        public async Task Validator_Should_ThrowValidationErrorException()
        {
            Task action() => Mediator.Send(new GetPostByIdQuery());

            await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
        }

        [Fact]
        public async Task Handler_Should_ReturnNotFoundException()
        {
            var result = await Mediator.Send(new GetPostByIdQuery { Id = Guid.NewGuid() });

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<NotFoundException>();
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithBlogInfo()
        {
            var result = await Mediator.Send(new GetPostByIdQuery { Id = PostId });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Should().NotBeNull();
            result.Value.Blog.Title.Should().Be("Title from configuration");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithPostCover()
        {
            var result = await Mediator.Send(new GetPostByIdQuery { Id = PostId });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Should().NotBeNull();
            result.Value.Post.Cover.Url.Should().Be("https://ofpinewood.com/cover-url");
        }

        [Fact]
        public async Task Handler_Should_ReturnPostModel_WithPostAuthor()
        {
            var result = await Mediator.Send(new GetPostByIdQuery { Id = PostId });

            result.IsSuccess.Should().BeTrue();
            result.Value.Post.Should().NotBeNull();
            result.Value.Post.Author.DisplayName.Should().Be("Author 1");
        }

        private void SeedDatabase()
        {
            var context = ServiceProvider.GetRequiredService<IBlogEntityDbContext>();

            var author = new Author { UserId = Guid.NewGuid(), DisplayName = "Author 1" };
            context.Authors.Add(author);
            context.SaveChanges();

            var post = CreatePost(0, author.Id);
            context.Posts.Add(post);
            context.SaveChanges();

            PostId = post.Id;

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
                Cover = new Cover
                {
                    Url = "https://ofpinewood.com/cover-url",
                    Caption = "Cover caption",
                    Link = "https://ofpinewood.com/cover-link"
                }
            };
        }
    }
}
