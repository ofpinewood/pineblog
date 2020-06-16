using FluentAssertions;
using Xunit;

namespace Opw.PineBlog.MongoDb
{
    public class BlogUnitOfWorkTests
    {
        [Fact]
        public void BlogUnitOfWork_Should_HaveInitializedAllRepositories()
        {
            var uow = new BlogUnitOfWork("mongodb://localhost:27017", "pineblog-db");

            uow.Should().NotBeNull();
            uow.BlogSettings.Should().NotBeNull();
            uow.Authors.Should().NotBeNull();
            uow.Posts.Should().NotBeNull();
        }
    }
}
