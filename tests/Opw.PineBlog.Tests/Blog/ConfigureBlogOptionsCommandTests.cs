using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Opw.PineBlog.Entities;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Blog
{
    public class ConfigureBlogOptionsCommandTests : MediatRTestsBase
    {
        [Fact]
        public async Task Handler_Should_AddSettingsToTheDatabase()
        {
            await Mediator.Send(new ConfigureBlogOptionsCommand());

            var context = ServiceProvider.GetRequiredService<IBlogEntityDbContext>();
            var settings = context.Settings.Single();

            settings.Title.Should().Be("Title from configuration");
        }

        [Fact]
        public async Task Handler_Should_SetOptionsFromDatabase()
        {
            var context = ServiceProvider.GetRequiredService<IBlogEntityDbContext>();
            var settings = new Settings
            {
                Title = "Title from database",
                Description = "Description from database",
                Cover = new Cover
                {
                    Url = "https://ofpinewood.com/cover-url",
                    Caption = "Cover caption",
                    Link = "https://ofpinewood.com/cover-link"
                }
            };
            context.Settings.Add(settings);
            await context.SaveChangesAsync(default);

            await Mediator.Send(new ConfigureBlogOptionsCommand());

            var options = ServiceProvider.GetRequiredService<IOptions<BlogOptions>>();

            options.Value.Title.Should().Be("Title from database");
            options.Value.Description.Should().Be("Description from database");
            options.Value.Cover.Url.Should().Be("https://ofpinewood.com/cover-url");
        }
    }
}
