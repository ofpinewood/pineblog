using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Areas.Admin.Pages
{
    public class AddPostModelTests : RazorPagesTestsBase
    {
        [Fact]
        public async Task OnGet_Should_SetEmptyAddPostCommand()
        {
            var loggerMock = new Mock<ILogger<AddPostModel>>();
            var mediaterMock = new Mock<IMediator>();
            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new AddPostModel(mediaterMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            var result = pageModel.OnGet();

            result.Should().BeOfType<PageResult>();
            pageModel.Post.Should().NotBeNull();
        }

        [Fact]
        public async Task OnPostAsync_Should_ReturnRedirectToPageResult()
        {
            var loggerMock = new Mock<ILogger<AddPostModel>>();
            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<Post>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Post>.Success(new Post()));

            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "user@example.com") }));
            var pageContext = GetPageContext(httpContext);

            var pageModel = new AddPostModel(mediaterMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2),
                Post = new AddPostCommand
                {
                    Categories = "category",
                    Title = "title",
                    Content = "content",
                    Description = "description"
                }
            };

            var result = await pageModel.OnPostAsync(default);

            result.Should().BeOfType<RedirectToPageResult>().Which.PageName.Should().Be("UpdatePost");
        }

        [Fact]
        public async Task OnPostAsync_Should_SetUserNameOfTheCurrentUser()
        {
            var loggerMock = new Mock<ILogger<AddPostModel>>();
            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<Post>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Post>.Success(new Post()));

            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "user@example.com") }));
            var pageContext = GetPageContext(httpContext);

            var pageModel = new AddPostModel(mediaterMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2),
                Post = new AddPostCommand
                {
                    Categories = "category",
                    Title = "title",
                    Content = "content",
                    Description = "description"
                }
            };

            var result = await pageModel.OnPostAsync(default);

            pageModel.Post.Should().NotBeNull();
            pageModel.Post.UserName.Should().Be("user@example.com");
        }

        [Fact]
        public async Task OnPostAsync_Should_SetModelStateError_OnAddPostCommandFail()
        {
            var loggerMock = new Mock<ILogger<AddPostModel>>();
            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<Post>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Post>.Fail(new ApplicationException("Error!")));

            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "user@example.com") }));
            var pageContext = GetPageContext(httpContext);

            var pageModel = new AddPostModel(mediaterMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2),
                Post = new AddPostCommand
                {
                    Categories = "category",
                    Title = "title",
                    Content = "content",
                    Description = "description"
                }
            };

            var result = await pageModel.OnPostAsync(default);

            result.Should().BeOfType<PageResult>();

            pageModel.ModelState.GetValueOrDefault("").Errors.First().ErrorMessage.Should().Be("Error!");
        }
    }
}
