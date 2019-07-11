using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Opw.PineBlog.Models;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Blog.Pages
{
    public class IndexModelTests : RazorPagesTestsBase
    {
        [Fact]
        public async Task OnGetAsync_Should_SetPostListModel()
        {
            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<PostListModel>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<PostListModel>.Success(new PostListModel {
                    Blog = new BlogModel(new BlogOptions()) { Title = "Blog Title" }
                }));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new IndexModel(mediaterMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            var result = await pageModel.OnGetAsync(default, 0);

            result.Should().BeOfType<PageResult>();
            pageModel.Model.Should().NotBeNull();
            pageModel.Title.Should().NotBeNull();
        }
    }
}
