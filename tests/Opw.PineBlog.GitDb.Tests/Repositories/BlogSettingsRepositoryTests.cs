using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Opw.PineBlog.Entities;
using Opw.PineBlog.GitDb.LibGit2;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.GitDb.Repositories
{
    public class BlogSettingsRepositoryTests : GitDbTestsBase
    {
        private readonly BlogSettingsRepository _blogSettingsRepository;
        private readonly IBlogUnitOfWork _uow;

        public BlogSettingsRepositoryTests(GitDbFixture fixture) : base(fixture)
        {
            _uow = ServiceProvider.GetRequiredService<IBlogUnitOfWork>();
            _blogSettingsRepository = (BlogSettingsRepository)_uow.BlogSettings;
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Should_ReturnBlogSettings()
        {
            var result = await _blogSettingsRepository.SingleOrDefaultAsync(CancellationToken.None);

            result.Should().NotBeNull();
            result.Title.Should().Be("PineBlog");
            result.Description.Should().Be("A blogging engine based on ASP.NET Core MVC Razor Pages and Entity Framework Core");
            result.CoverUrl.Should().Be("/images/woods.gif");
            result.CoverCaption.Should().Be("Battle background for the Misty Woods in the game Shadows of Adam by Tim Wendorf");
            result.CoverLink.Should().Be("http://pixeljoint.com/pixelart/94359.htm");
        }

        [Fact(Skip = "Checking out another branch while running the unit test will randomly fail other tests.")]
        public async Task SingleOrDefaultAsync_Should_ReturnNull_WhenNoBlogSettingsFile()
        {
            var options = new PineBlogGitDbOptions() { Branch = "test" };
            var optionsMock = new Mock<IOptionsSnapshot<PineBlogGitDbOptions>>();
            optionsMock.Setup(m => m.Value).Returns(options);
            var gitDbContext = ServiceProvider.GetRequiredService<GitDbContext>();

            var blogSettingsRepository = new BlogSettingsRepository(gitDbContext, optionsMock.Object);

            var result = await blogSettingsRepository.SingleOrDefaultAsync(CancellationToken.None);

            result.Should().BeNull();
        }

        [Fact]
        public void Add_Should_ThrowNotImplementedException()
        {
            Action act = () => _blogSettingsRepository.Add(new BlogSettings());

            act.Should().Throw<NotImplementedException>();
        }

        [Fact]
        public void Update_Should_ThrowNotImplementedException()
        {
            Action act = () => _blogSettingsRepository.Update(new BlogSettings());

            act.Should().Throw<NotImplementedException>();
        }
    }
}
