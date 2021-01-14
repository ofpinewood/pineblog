using Xunit;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;

namespace Opw.PineBlog.MongoDb
{
    public class ServiceCollectionExtensionsTests : MongoDbTestsBase
    {
        public ServiceCollectionExtensionsTests(MongoDbDatabaseFixture fixture) : base(fixture) { }

        [Fact(Skip = Constants.SkipMongoDbTests)]
        public void AddPineBlogMongoDb_Should_RegisterBlogUnitOfWork()
        {
            var uow = ServiceProvider.GetService<IBlogUnitOfWork>();

            uow.Should().NotBeNull();
        }
    }
}
