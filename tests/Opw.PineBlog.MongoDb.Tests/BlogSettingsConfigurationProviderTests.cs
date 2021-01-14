using Xunit;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Opw.PineBlog.Entities;

namespace Opw.PineBlog.MongoDb
{
    public class BlogSettingsConfigurationProviderTests : MongoDbTestsBase
    {
        private readonly BlogSettingsConfigurationProvider _provider;

        public BlogSettingsConfigurationProviderTests(MongoDbDatabaseFixture fixture) : base(fixture)
        {
            var connectionStringName = Configuration.GetSection(nameof(PineBlogOptions)).GetValue<string>(nameof(PineBlogOptions.ConnectionStringName));
            var databaseName = Configuration.GetSection(nameof(PineBlogOptions)).GetValue<string>(nameof(PineBlogOptions.MongoDbDatabaseName));
            var connectionString = Configuration.GetConnectionString(connectionStringName);

            _provider = new BlogSettingsConfigurationProvider(new BlogSettingsConfigurationSource
            {
                ConnectionString = connectionString,
                DatabaseName = databaseName,
                ReloadOnChange = true
            });
        }

        [Fact(Skip = Constants.SkipMongoDbBlogSettingsConfigurationProviderTests)]
        public void Load_Should_HaveSettings()
        {
            var uow = ServiceProvider.GetService<IBlogUnitOfWork>();
            uow.BlogSettings.Add(new BlogSettings { Title = "Test blog" });

            _provider.Load();

            _provider.TryGet($"{nameof(PineBlogOptions)}:{nameof(PineBlogOptions.Title)}", out var title);
            _provider.TryGet($"{nameof(PineBlogOptions)}:{nameof(PineBlogOptions.Description)}", out var description);

            title.Should().Be("Test blog");
            description.Should().BeNull();
        }

        [Fact(Skip = Constants.SkipMongoDbBlogSettingsConfigurationProviderTests)]
        public void Load_Should_NotHaveSettings()
        {
            _provider.Load();

            _provider.TryGet($"{nameof(PineBlogOptions)}:{nameof(PineBlogOptions.Title)}", out var title);

            title.Should().BeNull();
        }
    }
}
