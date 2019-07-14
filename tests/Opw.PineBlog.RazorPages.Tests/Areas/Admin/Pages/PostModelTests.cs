using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Opw.HttpExceptions;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Areas.Admin.Pages
{

    public class PostModelTests : RazorPagesTestsBase
    {
        [Fact]
        public async Task OnGetAsync_Should_SetPostModel()
        {
            var loggerMock = new Mock<ILogger<PostModel>>();

            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<SinglePostModel>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<SinglePostModel>.Success(new SinglePostModel
                {
                    Blog = new BlogModel(new BlogOptions()) { Title = "Blog Title" },
                    Post = new Post()
                }));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new PostModel(mediaterMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            var result = await pageModel.OnGetAsync(Guid.NewGuid(), default);

            result.Should().BeOfType<PageResult>();
            pageModel.Blog.Should().NotBeNull();
            pageModel.Post.Should().NotBeNull();
        }

        [Fact]
        public async Task OnGetAsync_Should_ThrowNotFoundException()
        {
            var loggerMock = new Mock<ILogger<PostModel>>();

            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<SinglePostModel>>>(), It.IsAny<CancellationToken>())).Throws<NotFoundException>();

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new PostModel(mediaterMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            Func<Task> action = async () => await pageModel.OnGetAsync(Guid.NewGuid(), default);

            action.Should().Throw<NotFoundException>();
        }
    }
}
