using FluentAssertions;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Opw.HttpExceptions;
using Opw.PineBlog.Entities;
using Opw.PineBlog.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Posts
{
    public class DeletePostCommandTests : MediatRTestsBase
    {
        private Guid _postId;

        public DeletePostCommandTests()
        {
            SeedDatabase();
        }

        [Fact]
        public async Task Validator_Should_ThrowValidationErrorException()
        {
            Task action() => Mediator.Send(new DeletePostCommand());

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(DeletePostCommand.Id))).Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnNotFoundException()
        {
            var result = await Mediator.Send(new DeletePostCommand { Id = Guid.NewGuid() });

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<NotFoundException<Post>>();
        }

        [Fact]
        public async Task Handler_Should_DeletePost()
        {
            var result = await Mediator.Send(new DeletePostCommand { Id = _postId });

            result.IsSuccess.Should().BeTrue();

            var context = ServiceProvider.GetRequiredService<BlogEntityDbContext>();

            var post = await context.Posts.SingleOrDefaultAsync(p => p.Id.Equals(_postId));

            post.Should().BeNull();
        }

        private void SeedDatabase()
        {
            var context = ServiceProvider.GetRequiredService<BlogEntityDbContext>();

            var author = new Author { UserName = "user@example.com", DisplayName = "Author 1" };
            context.Authors.Add(author);
            context.SaveChanges();

            var post = CreatePost(0, author.Id, true, false);
            context.Posts.Add(post);
            context.SaveChanges();

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
