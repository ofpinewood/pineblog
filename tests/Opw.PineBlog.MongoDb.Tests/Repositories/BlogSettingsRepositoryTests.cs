using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Opw.PineBlog.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.MongoDb.Repositories
{
    public class BlogSettingsRepositoryTests : MongoDbTestsBase
    {
        private readonly BlogSettingsRepository _blogSettingsRepository;
        private readonly IBlogUnitOfWork _uow;

        public BlogSettingsRepositoryTests()
        {
            _uow = ServiceProvider.GetRequiredService<IBlogUnitOfWork>();
            _blogSettingsRepository = (BlogSettingsRepository)_uow.BlogSettings;
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task SingleOrDefaultAsync_Should_ReturnBlogSettings()
        {
            BlogSettingsCollection.InsertOne(new BlogSettings
            {
                Title = "blog title",
                Description = "blog description",
                CoverCaption = "blog cover caption",
                CoverLink = "blog cover link",
                CoverUrl = "blog cover url",
                Created = DateTime.UtcNow,
            });

            var result = await _blogSettingsRepository.SingleOrDefaultAsync(CancellationToken.None);

            result.Should().NotBeNull();
            result.Title.Should().Be("blog title");
            result.Description.Should().Be("blog description");
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task SingleOrDefaultAsync_Should_ReturnNull()
        {
            var result = await _blogSettingsRepository.SingleOrDefaultAsync(CancellationToken.None);

            result.Should().BeNull();
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
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

            var result = BlogSettingsCollection.Find(Builders<BlogSettings>.Filter.Empty).SingleOrDefault();

            result.Should().NotBeNull();
            result.Title.Should().Be("blog title");
            result.Description.Should().Be("blog description");
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task Update_Should_UpdateBlogSettings()
        {
            BlogSettingsCollection.InsertOne(new BlogSettings
            {
                Title = "blog title",
                Description = "blog description",
                CoverCaption = "blog cover caption",
                CoverLink = "blog cover link",
                CoverUrl = "blog cover url",
                Created = DateTime.UtcNow,
            });

            var blogSettingsToUpdate = BlogSettingsCollection.Find(bs => true).SingleOrDefault();
            blogSettingsToUpdate.Should().NotBeNull();
            blogSettingsToUpdate.Title.Should().Be("blog title");

            blogSettingsToUpdate.Title = "UPDATED";

            _blogSettingsRepository.Update(blogSettingsToUpdate);
            await _uow.SaveChangesAsync(CancellationToken.None);

            var result = BlogSettingsCollection.Find(bs => true).SingleOrDefault();

            result.Should().NotBeNull();
            result.Title.Should().Be("UPDATED");
            result.Description.Should().Be("blog description");
        }
    }
}
