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
    public class AddPostCommandTests : MediatRTestsBase
    {
        public AddPostCommandTests()
        {
        }

        [Fact]
        public async Task Validator_Should_ThrowValidationErrorException()
        {
            await SeedDatabase();

            Task action() => Mediator.Send(new AddPostCommand());

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(AddPostCommand.UserName))).Should().NotBeNull();
            ex.Errors.Single(e => e.Key.Equals(nameof(AddPostCommand.Title))).Should().NotBeNull();
            ex.Errors.Single(e => e.Key.Equals(nameof(AddPostCommand.Content))).Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnNotFoundException_WhenInvalidUser()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new AddPostCommand
            {
                UserName = "invalid@example.com",
                Categories = "category",
                Title = "title",
                Content = "content",
                Description = "description"
            });

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<NotFoundException>();
        }

        [Fact]
        public async Task Handler_Should_AddPost()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new AddPostCommand
            {
                UserName = "user@example.com",
                Categories = "category",
                Title = "title",
                Content = "content",
                Description = "description",
                Published = DateTime.UtcNow
            });

            result.IsSuccess.Should().BeTrue();
            result.Value.Id.Should().NotBeEmpty();

            var repo = ServiceProvider.GetRequiredService<IRepository>();

            var post = await repo.GetPostBySlugAsync("title", CancellationToken.None);

            post.Should().NotBeNull();
            post.Id.Should().Be(result.Value.Id);
        }

        [Fact]
        public async Task Handler_Should_HaveSlug()
        {
            await SeedDatabase();

            var result = await Mediator.Send(new AddPostCommand
            {
                UserName = "user@example.com",
                Categories = "category",
                Title = "title or slug",
                Content = "content",
                Description = "description"
            });

            result.IsSuccess.Should().BeTrue();
            result.Value.Title.Should().Be("title or slug");
            result.Value.Slug.Should().MatchRegex(result.Value.Title.ToPostSlug());
        }

        private async Task SeedDatabase()
        {
            var repo = ServiceProvider.GetRequiredService<IRepository>();

            var author = new Author { UserName = "user@example.com", DisplayName = "Author 1" };
            await repo.AddAuthorAsync(author, CancellationToken.None);
        }
    }
}
