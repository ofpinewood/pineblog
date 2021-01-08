using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.EntityFrameworkCore.Repositories
{
    public class AuthorRepositoryTests : EntityFrameworkCoreTestsBase
    {
        private Guid _authorId;
        private readonly AuthorRepository _authorRepository;

        public AuthorRepositoryTests()
        {
            SeedDatabase();

            var uow = ServiceProvider.GetRequiredService<IBlogUnitOfWork>();
            _authorRepository = (AuthorRepository)uow.Authors;
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Should_ReturnAuthor()
        {
            var result = await _authorRepository.SingleOrDefaultAsync(a => a.Id == _authorId, CancellationToken.None);

            result.Should().NotBeNull();
            result.UserName.Should().Be("user@example.com");
            result.DisplayName.Should().Be("Author 1");
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Should_ReturnNull()
        {
            var result = await _authorRepository.SingleOrDefaultAsync(a => a.Id == Guid.NewGuid(), CancellationToken.None);

            result.Should().BeNull();
        }

        private void SeedDatabase()
        {
            var context = ServiceProvider.GetRequiredService<BlogEntityDbContext>();

            var author = new Author { UserName = "user@example.com", DisplayName = "Author 1" };
            context.Authors.Add(author);
            context.SaveChanges();

            _authorId = author.Id;
        }
    }
}
