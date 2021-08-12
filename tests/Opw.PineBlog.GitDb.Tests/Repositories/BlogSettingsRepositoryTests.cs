using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.Entities;
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

        public BlogSettingsRepositoryTests()
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

        //[Fact]
        //public async Task SingleOrDefaultAsync_Should_ReturnNull()
        //{
        //    var result = await _blogSettingsRepository.SingleOrDefaultAsync(CancellationToken.None);

        //    result.Should().BeNull();
        //}

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
