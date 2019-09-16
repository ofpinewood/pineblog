using FluentAssertions;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Opw.HttpExceptions;
using Opw.PineBlog.Entities;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Blogs
{
    public class UpdateBlogSettingsCommandTests : MediatRTestsBase
    {
        public UpdateBlogSettingsCommandTests()
        {
            SeedDatabase();
        }
        
        [Fact]
        public async Task Validator_Should_ThrowValidationErrorException()
        {
            Task action() => Mediator.Send(new UpdateBlogSettingsCommand());

            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
            ex.Errors.Single(e => e.Key.Equals(nameof(UpdateBlogSettingsCommand.Title))).Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_Should_ReturnNotFoundException()
        {
            var context = ServiceProvider.GetRequiredService<IBlogEntityDbContext>();

            var existing = await context.BlogSettings.SingleAsync();
            context.BlogSettings.Remove(existing);
            await context.SaveChangesAsync(true, default);

            var result = await Mediator.Send(new UpdateBlogSettingsCommand
            {
                Title = "title"
            });

            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<NotFoundException<BlogSettings>>();
        }

        [Fact]
        public async Task Handler_Should_UpdatePost()
        {
            var result = await Mediator.Send(new UpdateBlogSettingsCommand
            {
                Title = "blog title-UPDATED",
                Description = "blog description",
                CoverCaption = "blog cover caption",
                CoverLink = "blog cover link",
                CoverUrl = "blog cover url"
            });

            result.IsSuccess.Should().BeTrue();

            var context = ServiceProvider.GetRequiredService<IBlogEntityDbContext>();

            var settings = await context.BlogSettings.SingleAsync();

            settings.Should().NotBeNull();
            settings.Title.Should().Be("blog title-UPDATED");
        }

        private void SeedDatabase()
        {
            var context = ServiceProvider.GetRequiredService<IBlogEntityDbContext>();

            context.BlogSettings.Add(new BlogSettings {
                Title = "blog title",
                Description = "blog description",
                CoverCaption = "blog cover caption",
                CoverLink = "blog cover link",
                CoverUrl = "blog cover url"
            });
            context.SaveChanges();
        }
    }
}
