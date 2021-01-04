using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.EntityFrameworkCore.Repositories
{
    public class BlogSettingsRepositoryTests : EntityFrameworkCoreTestsBase
    {
        private readonly BlogSettingsRepository _blogSettingsRepository;
        private readonly IBlogUnitOfWork _uow;
        private readonly BlogEntityDbContext _dbContext;

        public BlogSettingsRepositoryTests()
        {
            _dbContext = ServiceProvider.GetRequiredService<BlogEntityDbContext>();
            _uow = ServiceProvider.GetRequiredService<IBlogUnitOfWork>();
            _blogSettingsRepository = (BlogSettingsRepository)_uow.BlogSettings;
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Should_ReturnBlogSettings()
        {
            _dbContext.BlogSettings.Add(new BlogSettings
            {
                Title = "blog title",
                Description = "blog description",
                CoverCaption = "blog cover caption",
                CoverLink = "blog cover link",
                CoverUrl = "blog cover url"
            });
            _dbContext.SaveChanges();

            var result = await _blogSettingsRepository.SingleOrDefaultAsync(CancellationToken.None);

            result.Should().NotBeNull();
            result.Title.Should().Be("blog title");
            result.Description.Should().Be("blog description");
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Should_ReturnNull()
        {
            var result = await _blogSettingsRepository.SingleOrDefaultAsync(CancellationToken.None);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Add_Should_AddBlogSettings()
        {
            var blogSettings = new BlogSettings
            {
                Title = "blog title",
                Description = "blog description",
                CoverCaption = "blog cover caption",
                CoverLink = "blog cover link",
                CoverUrl = "blog cover url"
            };

            _blogSettingsRepository.Add(blogSettings);
            await _uow.SaveChangesAsync(CancellationToken.None);

            var result = _dbContext.BlogSettings.SingleOrDefault();

            result.Should().NotBeNull();
            result.Title.Should().Be("blog title");
            result.Description.Should().Be("blog description");
        }

        [Fact]
        public async Task Update_Should_UpdateBlogSettings()
        {
            var blogSettings = new BlogSettings
            {
                Title = "blog title",
                Description = "blog description",
                CoverCaption = "blog cover caption",
                CoverLink = "blog cover link",
                CoverUrl = "blog cover url"
            };
            _dbContext.BlogSettings.Add(blogSettings);
            _dbContext.SaveChanges();

            var result = _dbContext.BlogSettings.SingleOrDefault();
            result.Should().NotBeNull();
            result.Title.Should().Be("blog title");

            blogSettings.Title = "UPDATED";

            _blogSettingsRepository.Update(blogSettings);
            await _uow.SaveChangesAsync(CancellationToken.None);

            result = _dbContext.BlogSettings.SingleOrDefault();

            result.Should().NotBeNull();
            result.Title.Should().Be("UPDATED");
            result.Description.Should().Be("blog description");
        }
    }
}
