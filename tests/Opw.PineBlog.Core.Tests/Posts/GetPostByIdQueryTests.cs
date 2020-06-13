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
    public class GetPostByIdQueryTests : MediatRTestsBase
    {
        private Guid _postId;

        public GetPostByIdQueryTests()
        {
        }

        [Fact]
        public async Task Validator_Should_ThrowValidationErrorException()
        {
            await SeedDatabase();

            Task action() => Mediator.Send(new GetPostByIdQuery());

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(GetPostByIdQuery.Id))).Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnNotFoundException()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPostByIdQuery { Id = Guid.NewGuid() });

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<NotFoundException<Post>>();
        }

        [Fact]
        public async Task Handler_Should_ReturnPost()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new GetPostByIdQuery { Id = _postId });

            result.IsSuccess.Should().BeTrue();
            result.Value.Title.Should().Be("Post title 0");
        }

        private async Task SeedDatabase()
        {
            var repo = ServiceProvider.GetRequiredService<IRepository>();

            var author = new Author { UserName = "user@example.com", DisplayName = "Author 1" };
            await repo.AddAuthorAsync(author, CancellationToken.None);

            var post = CreatePost(0, author.Id);
            await repo.AddPostAsync(post, CancellationToken.None);

            _postId = post.Id;
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
