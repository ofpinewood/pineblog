using Xunit;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;

namespace Opw.PineBlog.Resume.EntityFrameworkCore
{
    public class ServiceCollectionExtensionsTests : EntityFrameworkCoreTestsBase
    {
        [Fact]
        public void AddPineBlogResumeEntityFrameworkCore_Should_RegisterResumeEntityDbContext()
        {
            var context = ServiceProvider.GetService<IResumeEntityDbContext>();

            context.Should().NotBeNull();
        }
    }
}
