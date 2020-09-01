using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Opw.PineBlog.EntityFrameworkCore
{
    public class BlogUnitOfWorkTests
    {
        [Fact]
        public void BlogUnitOfWork_Should_HaveInitializedAllRepositories()
        {
            var uow = new BlogUnitOfWork(new BlogEntityDbContext(new DbContextOptions<BlogEntityDbContext>()));

            uow.Should().NotBeNull();
            uow.BlogSettings.Should().NotBeNull();
            uow.Authors.Should().NotBeNull();
            uow.Posts.Should().NotBeNull();
        }
    }
}
