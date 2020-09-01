using Xunit;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Opw.EntityFrameworkCore;
using Opw.PineBlog.Entities;

namespace Opw.PineBlog.EntityFrameworkCore
{
    public class BlogSettingsConfigurationProviderTests : EntityFrameworkCoreTestsBase
    {
        private readonly BlogSettingsConfigurationProvider _provider;

        public BlogSettingsConfigurationProviderTests() : base()
        {
            var connectionStringName = Configuration.GetSection(nameof(PineBlogOptions)).GetValue<string>(nameof(PineBlogOptions.ConnectionStringName));
            var connectionString = Configuration.GetConnectionString(connectionStringName);

            var optionsAction = DbContextOptionsHelper.Configure(connectionString);
            _provider = new BlogSettingsConfigurationProvider(new BlogSettingsConfigurationSource { OptionsAction = optionsAction });
        }

        [Fact]
        public void Load_Should_HaveSettings()
        {
            var context = ServiceProvider.GetService<BlogEntityDbContext>();
            context.BlogSettings.Add(new BlogSettings { Title = "Test blog" });
            context.SaveChanges();

            _provider.Load();

            _provider.TryGet($"{nameof(PineBlogOptions)}:{nameof(PineBlogOptions.Title)}", out var title);
            _provider.TryGet($"{nameof(PineBlogOptions)}:{nameof(PineBlogOptions.Description)}", out var description);

            title.Should().Be("Test blog");
            description.Should().BeNull();
        }

        [Fact]
        public void Load_Should_NotHaveSettings()
        {
            _provider.Load();

            _provider.TryGet($"{nameof(PineBlogOptions)}:{nameof(PineBlogOptions.Title)}", out var title);

            title.Should().BeNull();
        }
    }
}
