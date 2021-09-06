using Xunit;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.Options;

namespace Opw.PineBlog.GitDb
{
    public class BlogSettingsConfigurationProviderTests : GitDbTestsBase
    {
        private readonly IOptions<PineBlogGitDbOptions> _options;
        private readonly BlogSettingsConfigurationProvider _provider;

        public BlogSettingsConfigurationProviderTests(GitDbFixture fixture) : base(fixture)
        {
            _options = ServiceProvider.GetRequiredService<IOptions<PineBlogGitDbOptions>>();

            _provider = new BlogSettingsConfigurationProvider(new BlogSettingsConfigurationSource
            {
                Options = _options.Value,
                ReloadOnChange = true
            });
        }

        [Fact(Skip = Constants.SkipGitDbBlogSettingsConfigurationProviderTests)]
        public void Load_Should_HaveSettings()
        {
            _provider.Load();

            _provider.TryGet($"{nameof(PineBlogOptions)}:{nameof(PineBlogOptions.Title)}", out var title);
            _provider.TryGet($"{nameof(PineBlogOptions)}:{nameof(PineBlogOptions.Description)}", out var description);

            title.Should().Be("PineBlog");
            description.Should().Be("A blogging engine based on ASP.NET Core MVC Razor Pages and Entity Framework Core");
        }

        [Fact(Skip = Constants.SkipGitDbBlogSettingsConfigurationProviderTests)]
        public void Load_Should_NotHaveSettings()
        {
            var options = _options.Value;
            options.RootPath = "invalid";

            var provider = new BlogSettingsConfigurationProvider(new BlogSettingsConfigurationSource
            {
                Options = _options.Value,
                ReloadOnChange = true
            });

            provider.Load();

            provider.TryGet($"{nameof(PineBlogOptions)}:{nameof(PineBlogOptions.Title)}", out var title);

            title.Should().BeNull();
        }
    }
}
