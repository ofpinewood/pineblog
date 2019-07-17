using FluentAssertions;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Opw.HttpExceptions;
using Opw.PineBlog.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Posts
{
    public class AddPostCommandTests : MediatRTestsBase
    {
        public AddPostCommandTests()
        {
            SeedDatabase();
        }
        
        [Fact]
        public async Task Validator_Should_ThrowValidationErrorException()
        {
            Task action() => Mediator.Send(new AddPostCommand());

            await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
        }

        [Fact]
        public async Task Handler_Should_ReturnNotFoundException_WhenInvalidUser()
        {
            var result = await Mediator.Send(new AddPostCommand {
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
            var result = await Mediator.Send(new AddPostCommand
            {
                UserName = "user@example.com",
                Categories = "category",
                Title = "title",
                Content = "content",
                Description = "description"
            });

            result.IsSuccess.Should().BeTrue();
            result.Value.Id.Should().NotBeEmpty();

            var context = ServiceProvider.GetRequiredService<IBlogEntityDbContext>();

            var post = await context.Posts.SingleAsync(p => p.Title.Equals("title"));

            post.Should().NotBeNull();
            post.Id.Should().Be(result.Value.Id);
        }

        [Fact]
        public async Task Handler_Should_SetSlug()
        {
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
            result.Value.Slug.Should().Be("title-or-slug");
        }

        private void SeedDatabase()
        {
            var context = ServiceProvider.GetRequiredService<IBlogEntityDbContext>();

            var author = new Author { UserName = "user@example.com", DisplayName = "Author 1" };
            context.Authors.Add(author);
            context.SaveChanges();
        }
    }
}
