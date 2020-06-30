using FluentAssertions;
using Opw.PineBlog.Entities;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using MongoDB.Driver;
using Moq;

namespace Opw.PineBlog.MongoDb
{
    public class BlogUnitOfWorkTests
    {
        private readonly Mock<IMongoDatabase> _mongoDatabaseMock;
        private readonly BlogUnitOfWork _uow;

        public BlogUnitOfWorkTests()
        {
            _mongoDatabaseMock = new Mock<IMongoDatabase>();
            _mongoDatabaseMock.Setup(m => m.GetCollection<Post>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>()))
                .Returns(new Mock<IMongoCollection<Post>>().Object);

            _uow = new BlogUnitOfWork(_mongoDatabaseMock.Object);
        }

        [Fact]
        public void BlogUnitOfWork_Should_HaveInitializedAllRepositories()
        {
            _uow.Should().NotBeNull();
            _uow.BlogSettings.Should().NotBeNull();
            _uow.Authors.Should().NotBeNull();
            _uow.Posts.Should().NotBeNull();
        }

        [Fact]
        public async Task SaveChangesAsync_Should_Return2()
        {
            _uow.Posts.Add(new Post());
            _uow.Posts.Add(new Post());
            var result = await _uow.SaveChangesAsync(CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(2);
        }

        [Fact]
        public async Task SaveChangesAsync_Should_ResetSaveChangeCount()
        {
            _uow.Posts.Add(new Post());
            _uow.Posts.Add(new Post());
            var result = await _uow.SaveChangesAsync(CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(2);

            result = await _uow.SaveChangesAsync(CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(0);
        }
    }
}
