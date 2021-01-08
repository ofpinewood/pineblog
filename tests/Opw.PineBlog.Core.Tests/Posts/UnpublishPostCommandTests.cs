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
    public class UnpublishPostCommandTests : MediatRTestsBase
    {
        private Guid _postId = Guid.NewGuid();

        public UnpublishPostCommandTests()
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
            Task action() => Mediator.Send(new UnpublishPostCommand());

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(UnpublishPostCommand.Id))).Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnNotFoundException()
        {
            PostRepositoryMock.Setup(m => m.SingleOrDefaultAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(Post));

            var result = await Mediator.Send(new UnpublishPostCommand { Id = Guid.NewGuid() });

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<NotFoundException<Post>>();
        }

        [Fact]
        public async Task Handler_Should_UnpublishPost()
        {
            var result = await Mediator.Send(new UnpublishPostCommand { Id = _postId });

            result.IsSuccess.Should().BeTrue();
            result.Should().NotBeNull();
            result.Value.Published.Should().BeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnExceptionResult_WhenSaveChangesError()
        {
            BlogUnitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Result<int>.Fail(new ApplicationException("Error: SaveChangesAsync")));

            var result = await Mediator.Send(new UnpublishPostCommand { Id = _postId });

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<ApplicationException>();
        }
    }
}
