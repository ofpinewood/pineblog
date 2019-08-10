using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.RazorPages.Areas.Admin.Pages
{
    public class PostsModelTests : RazorPagesTestsBase
    {
        [Fact]
        public async Task OnGetAsync_Should_SetPostListModel()
        {
            var loggerMock = new Mock<ILogger<PostsModel>>();
            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<PostListModel>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<PostListModel>.Success(new PostListModel {
                    Blog = new BlogModel(new PineBlogOptions()) { Title = "Blog Title" },
                    Pager = new Pager(0),
                    Posts = new List<Post>()
                }));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new PostsModel(mediaterMock.Object, OptionsMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            var result = await pageModel.OnGetAsync(default, 0);

            result.Should().BeOfType<PageResult>();
            pageModel.Pager.Should().NotBeNull();
            pageModel.Posts.Should().NotBeNull();
        }
    }
}
