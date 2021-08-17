using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Opw.PineBlog.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.GitDb.Repositories
{
    public class AuthorRepositoryTests : GitDbTestsBase
    {
        private readonly AuthorRepository _authorRepository;
        private readonly IBlogUnitOfWork _uow;

        public AuthorRepositoryTests()
        {
            _uow = ServiceProvider.GetRequiredService<IBlogUnitOfWork>();
            _authorRepository = (AuthorRepository)_uow.Authors;
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Should_ReturnAuthor_ForExistingUserName()
        {
            var result = await _authorRepository.SingleOrDefaultAsync(a => a.UserName == "john.smith@example.com", CancellationToken.None);

            result.Should().NotBeNull();
            result.UserName.Should().Be("john.smith@example.com");
            result.Email.Should().Be("john.smith@example.com");
            result.DisplayName.Should().Be("John Smith");
            result.Bio.Should().Be("The bio of John Smith");
            result.Avatar.Should().Be("images/avatar-male.png");
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Should_ReturnNull_ForNonExistingUserName()
        {
            var result = await _authorRepository.SingleOrDefaultAsync(a => a.UserName == "invalid@example.com", CancellationToken.None);

            result.Should().BeNull();
        }
    }
}
