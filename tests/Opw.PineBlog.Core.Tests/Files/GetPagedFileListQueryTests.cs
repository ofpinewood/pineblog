using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Files
{
    public class GetPagedFileListQueryTests : MediatRTestsBase
    {
        [Fact]
        public async Task Handler_Should_ReturnFileListModel_WithPagerItemsPerPageValue_SetFromRequest()
        {
            var result = await Mediator.Send(new GetPagedFileListQuery { Page = 1, ItemsPerPage = 5, DirectoryPath = "files" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Pager.ItemsPerPage.Should().Be(5);
        }

        [Fact]
        public async Task Handler_Should_ReturnFileListModel_WithPagerItemsPerPage_SetFromBlogOptions()
        {
            var result = await Mediator.Send(new GetPagedFileListQuery { Page = 1, DirectoryPath = "files" });

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Pager.ItemsPerPage.Should().Be(3);
        }
    }
}
