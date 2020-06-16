using FluentAssertions;
using Opw.PineBlog.Entities;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.Repositories;
using System;

namespace Opw.PineBlog.MongoDb.Repositories
{
    public class AuthorRepositoryTests : MongoDbTestsBase
    {
        private readonly IAuthorRepository _repository;

        public AuthorRepositoryTests()
        {
            AuthorCollection.InsertOne(new Author
            {
                Id = Guid.NewGuid(),
                DisplayName = "Bob Ross",
                UserName = "bob.ross@example.com"
            });

            var uow = ServiceProvider.GetService<IBlogUnitOfWork>();
            _repository = uow.Authors;
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task SingleOrDefaultAsync_Should_Return1Author()
        {
            var result = await _repository.SingleOrDefaultAsync(a => a.UserName == "bob.ross@example.com", CancellationToken.None);

            result.Should().NotBeNull();
            result.DisplayName.Should().Be("Bob Ross");
        }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public async Task SingleOrDefaultAsync_Should_Return0Authors()
        {
            var result = await _repository.SingleOrDefaultAsync(a => a.UserName == "invalid@example.com", CancellationToken.None);

            result.Should().BeNull();
        }
    }
}
