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
    public class DeletePostCommandTests : MediatRTestsBase
    {
        private Guid _postId = Guid.NewGuid();

        public DeletePostCommandTests()
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
            Task action() => Mediator.Send(new DeletePostCommand());

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(DeletePostCommand.Id))).Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnNotFoundException()
        {
            PostRepositoryMock.Setup(m => m.SingleOrDefaultAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(Post));

            var result = await Mediator.Send(new DeletePostCommand { Id = Guid.NewGuid() });

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<NotFoundException<Post>>();
        }

        [Fact]
        public async Task Handler_Should_DeletePost()
        {
            Post deletedPost = null;

            PostRepositoryMock.Setup(m => m.Remove(It.IsAny<Post>())).Callback((Post p) => deletedPost = p);
            
            var result = await Mediator.Send(new DeletePostCommand { Id = _postId });

            result.IsSuccess.Should().BeTrue();

            deletedPost.Should().NotBeNull();
            deletedPost.Published.Should().BeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnExceptionResult_WhenSaveChangesError()
        {
            BlogUnitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Result<int>.Fail(new ApplicationException("Error: SaveChangesAsync")));

            var result = await Mediator.Send(new DeletePostCommand { Id = _postId });

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<ApplicationException>();
        }
    }
}
