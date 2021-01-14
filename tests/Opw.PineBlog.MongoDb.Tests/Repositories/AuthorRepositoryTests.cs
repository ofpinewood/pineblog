using FluentAssertions;
using Opw.PineBlog.Entities;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Opw.PineBlog.MongoDb.Repositories
{
    public class AuthorRepositoryTests : MongoDbTestsBase
    {
        private readonly AuthorRepository _repository;

        public AuthorRepositoryTests(MongoDbDatabaseFixture fixture) : base(fixture)
        {
            SeedDatabase();

            var uow = ServiceProvider.GetService<IBlogUnitOfWork>();
            _repository = (AuthorRepository)uow.Authors;
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task SingleOrDefaultAsync_Should_ReturnAuthor()
        {
            var result = await _repository.SingleOrDefaultAsync(a => a.UserName == "user@example.com", CancellationToken.None);

            result.Should().NotBeNull();
            result.DisplayName.Should().Be("Author 1");
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task SingleOrDefaultAsync_Should_ReturnNull()
        {
            var result = await _repository.SingleOrDefaultAsync(a => a.UserName == "invalid@example.com", CancellationToken.None);

            result.Should().BeNull();
        }

        private void SeedDatabase()
        {
            AuthorCollection.InsertOne(new Author
            {
                Id = Guid.NewGuid(),
                DisplayName = "Author 1",
                UserName = "user@example.com"
            });
        }
    }
}
