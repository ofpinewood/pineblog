using FluentAssertions;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Opw.HttpExceptions;
using Opw.PineBlog.Entities;
using System.Linq;
using System.Threading;
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
        public async Task Handler_Should_AddSettings_WhenNotFound()
        {
            var repo = ServiceProvider.GetRequiredService<IRepository>();

            var existing = await repo.GetBlogSettingsAsync(CancellationToken.None);
            await repo.DeleteBlogSettingsAsync(existing, CancellationToken.None);

            var result = await Mediator.Send(new UpdateBlogSettingsCommand
            {
                Title = "blog title-NEW"
            });

            result.IsSuccess.Should().BeTrue();

            repo = ServiceProvider.GetRequiredService<IRepository>();

            var settings = await repo.GetBlogSettingsAsync(CancellationToken.None);

            settings.Should().NotBeNull();
            settings.Title.Should().Be("blog title-NEW");
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

            var repo = ServiceProvider.GetRequiredService<IRepository>();

            var settings = await repo.GetBlogSettingsAsync(CancellationToken.None);

            settings.Should().NotBeNull();
            settings.Title.Should().Be("blog title-UPDATED");
        }

        private async Task SeedDatabase()
        {
            var repo = ServiceProvider.GetRequiredService<IRepository>();

            await repo.UpdateBlogSettingsAsync(new BlogSettings
            {
                Title = "blog title",
                Description = "blog description",
                CoverCaption = "blog cover caption",
                CoverLink = "blog cover link",
                CoverUrl = "blog cover url"
            }, CancellationToken.None);
        }
    }
}
