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
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.RazorPages.Areas.Admin.Pages
{
    public class UpdatePostModelTests : RazorPagesTestsBase
    {
        private readonly Guid _guid = Guid.Parse("0fcc6469-9e9a-4dd6-9b69-869d0f66e422");

        [Fact]
        public async Task OnGetAsync_Should_SetUpdatePostCommand()
        {
            var loggerMock = new Mock<ILogger<UpdatePostModel>>();
            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<GetPostByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Post>.Success(new Post { Id = _guid }));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new UpdatePostModel(mediaterMock.Object, FeatureManagerMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            var result = await pageModel.OnGetAsync(_guid, default);

            result.Should().BeOfType<PageResult>();
            pageModel.Post.Id.Should().Be(_guid);
        }

        [Fact]
        public async Task OnPostAsync_Should_ReturnSuccess()
        {
            var loggerMock = new Mock<ILogger<UpdatePostModel>>();
            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<UpdatePostCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Post>.Success(new Post { Id = _guid }));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new UpdatePostModel(mediaterMock.Object, FeatureManagerMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2),
                Post = new UpdatePostCommand
                {
                    Id = _guid,
                    Categories = "category",
                    Title = "title",
                    Content = "content",
                    Description = "description"
                }
            };

            var result = await pageModel.OnPostAsync(default);

            result.Should().BeOfType<PageResult>();
            pageModel.ModelState.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task OnGetPublishAsync_Should_ReturnRedirectToPageResult()
        {
            var loggerMock = new Mock<ILogger<UpdatePostModel>>();
            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<PublishPostCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Post>.Success(new Post { Id = _guid }));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new UpdatePostModel(mediaterMock.Object, FeatureManagerMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            var result = await pageModel.OnGetPublishAsync(_guid, default);

            result.Should().BeOfType<RedirectToPageResult>()
                .Which.PageName.Should().Be("UpdatePost");
        }

        [Fact]
        public async Task OnGetUnpublishAsync_Should_ReturnRedirectToPageResult()
        {
            var loggerMock = new Mock<ILogger<UpdatePostModel>>();
            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<UnpublishPostCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Post>.Success(new Post { Id = _guid }));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new UpdatePostModel(mediaterMock.Object, FeatureManagerMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            var result = await pageModel.OnGetUnpublishAsync(_guid, default);

            result.Should().BeOfType<RedirectToPageResult>()
                .Which.PageName.Should().Be("UpdatePost");
        }

        [Fact]
        public async Task OnGetDeleteAsync_Should_ReturnRedirectToPageResult()
        {
            var loggerMock = new Mock<ILogger<UpdatePostModel>>();
            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<DeletePostCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success());

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new UpdatePostModel(mediaterMock.Object, FeatureManagerMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            var result = await pageModel.OnGetDeleteAsync(_guid, default);

            result.Should().BeOfType<RedirectToPageResult>()
                .Which.PageName.Should().Be("Posts");
        }
    }
}
