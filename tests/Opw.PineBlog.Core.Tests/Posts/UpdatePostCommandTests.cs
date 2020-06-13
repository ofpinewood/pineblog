using FluentAssertions;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Opw.HttpExceptions;
using Opw.PineBlog.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Posts
{
    public class UpdatePostCommandTests : MediatRTestsBase
    {
        private Guid _postId;

        public UpdatePostCommandTests()
        {
        }

        [Fact]
        public async Task Validator_Should_ThrowValidationErrorException()
        {
            await SeedDatabase();

            Task action() => Mediator.Send(new UpdatePostCommand());

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(UpdatePostCommand.Id))).Should().NotBeNull();
            ex.Errors.Single(e => e.Key.Equals(nameof(UpdatePostCommand.Title))).Should().NotBeNull();
            ex.Errors.Single(e => e.Key.Equals(nameof(UpdatePostCommand.Content))).Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnNotFoundException()
        {
            await SeedDatabase();

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
            await SeedDatabase();

            var result = await Mediator.Send(new UpdatePostCommand
            {
                Id = _postId,
                Categories = "category",
                Title = "title-UPDATED",
                Content = "content",
                Description = "description"
            });

            result.IsSuccess.Should().BeTrue();

            var repo = ServiceProvider.GetRequiredService<IRepository>();

            var post = await repo.GetPostByIdAsync(_postId, CancellationToken.None);

            post.Should().NotBeNull();
            post.Title.Should().Be("title-UPDATED");
        }

        private async Task SeedDatabase()
        {
            var repo = ServiceProvider.GetRequiredService<IRepository>();

            var author = new Author { UserName = "user@example.com", DisplayName = "Author 1" };
            await repo.AddAuthorAsync(author, CancellationToken.None);

            var post = CreatePost(0, author.Id, true, false);
            await repo.AddPostAsync(post, CancellationToken.None);

            _postId = post.Id;
        }

        private Post CreatePost(int i, Guid authorId, bool published, bool cover)
        {
            var post = new Post
            {
                AuthorId = authorId,
                Title = "Post title " + i,
                Slug = "post-title-" + i,
                Description = "Description",
                Content = "Content"
            };

            if (published) post.Published = DateTime.UtcNow;
            if (cover)
            {
                post.CoverUrl = "https://ofpinewood.com/cover-url";
                post.CoverCaption = "Cover caption";
                post.CoverLink = "https://ofpinewood.com/cover-link";
            }

            return post;
        }
    }
}
