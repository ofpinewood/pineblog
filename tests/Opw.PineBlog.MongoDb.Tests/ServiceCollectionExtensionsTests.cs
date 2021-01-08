using Xunit;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;

namespace Opw.PineBlog.MongoDb
{
    public class ServiceCollectionExtensionsTests : MongoDbTestsBase
    {
        [Fact]
        public void AddPineBlogMongoDb_Should_RegisterBlogUnitOfWork()
        {
            var uow = ServiceProvider.GetService<IBlogUnitOfWork>();

            uow.Should().NotBeNull();
        }
    }
}
