using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Opw.HttpExceptions;
using Opw.PineBlog.Areas.Blog.Pages;
using Opw.PineBlog.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Blog.Pages
{

    public class PostModelTests : RazorPagesTestsBase
    {
        [Fact]
        public async Task OnGetAsync_Should_SetPostModel()
        {
            var loggerMock = new Mock<ILogger<Areas.Blog.Pages.PostModel>>();

            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<Models.PostModel>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Models.PostModel>.Success(new Models.PostModel
                {
                    Blog = new BlogModel(new BlogOptions()) { Title = "Blog Title" }
                }));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new Areas.Blog.Pages.PostModel(mediaterMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            var result = await pageModel.OnGetAsync("slug", default);

            result.Should().BeOfType<PageResult>();
            pageModel.Model.Should().NotBeNull();
            pageModel.Title.Should().NotBeNull();
        }

        [Fact]
        public async Task OnGetAsync_Should_ThrowNotFoundException()
        {
            var loggerMock = new Mock<ILogger<Areas.Blog.Pages.PostModel>>();

            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<Models.PostModel>>>(), It.IsAny<CancellationToken>())).Throws<NotFoundException>();

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new Areas.Blog.Pages.PostModel(mediaterMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            Func<Task> action = async () => await pageModel.OnGetAsync("slug", default);

            action.Should().Throw<NotFoundException>();
        }
    }
}
