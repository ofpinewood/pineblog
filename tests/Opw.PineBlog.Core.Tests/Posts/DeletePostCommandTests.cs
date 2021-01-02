using FluentAssertions;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
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
    public class DeletePostCommandTests : MediatRTestsBase
    {
        private Guid _postId = Guid.NewGuid();

        public DeletePostCommandTests()
        {
            var author = new Author { UserName = "user@example.com", DisplayName = "Author 1" };

            AuthorRepositoryMock.Setup(m => m.SingleOrDefaultAsync(It.IsAny<Expression<Func<Author, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(author);

            var posts = new List<Post>();
            posts.Add(CreatePost(0, _postId, Guid.NewGuid(), true, false));

            PostRepositoryMock.Setup(m => m.SingleOrDefaultAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(posts.SingleOrDefault());

            AddBlogUnitOfWorkMock();
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
            PostRepositoryMock.Setup(m => m.SingleOrDefaultAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(Post));

            AddBlogUnitOfWorkMock();

            var result = await Mediator.Send(new DeletePostCommand { Id = Guid.NewGuid() });

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<NotFoundException<Post>>();
        }

        [Fact]
        public async Task Handler_Should_DeletePost()
        {
            Post deletedPost = null;

            PostRepositoryMock.Setup(m => m.Remove(It.IsAny<Post>())).Callback((Post p) => deletedPost = p);
            AddBlogUnitOfWorkMock();

            var result = await Mediator.Send(new DeletePostCommand { Id = _postId });

            result.IsSuccess.Should().BeTrue();

            deletedPost.Should().NotBeNull();
            deletedPost.Published.Should().BeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnExceptionResult_WhenSaveChangesError()
        {
            BlogUnitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Result<int>.Fail(new ApplicationException("Error: SaveChangesAsync")));

            AddBlogUnitOfWorkMock();

            var result = await Mediator.Send(new DeletePostCommand { Id = _postId });

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<ApplicationException>();
        }

        private Post CreatePost(int i, Guid postId, Guid authorId, bool published, bool cover)
        {
            var post = new Post
            {
                Id = postId,
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
