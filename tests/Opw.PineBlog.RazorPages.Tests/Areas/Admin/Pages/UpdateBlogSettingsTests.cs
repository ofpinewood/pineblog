using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Opw.PineBlog.Blogs;
using Opw.PineBlog.Entities;
using Opw.PineBlog.FeatureManagement;
using Opw.PineBlog.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.RazorPages.Areas.Admin.Pages
{
    public class UpdateBlogSettingsTests : RazorPagesTestsBase
    {
        [Fact]
        public async Task OnGetAsync_Should_SetPostListModel()
        {
            var loggerMock = new Mock<ILogger<UpdateBlogSettingsModel>>();
            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<BlogSettings>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<BlogSettings>.Success(new BlogSettings
                {
                    Title = "Blog Title",
                }));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new UpdateBlogSettingsModel(mediaterMock.Object, FeatureManagerMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            var result = await pageModel.OnGetAsync(default);

            result.Should().BeOfType<PageResult>();
            pageModel.BlogSettings.Should().NotBeNull();
        }

        [Fact]
        public async Task OnGetAsync_Should_HaveFeatureStateDisabled()
        {
            FeatureManagerMock
                .Setup(m => m.IsEnabled(It.IsAny<FeatureFlag>()))
                .Returns(FeatureState.Disabled("Disabled!"));

            var loggerMock = new Mock<ILogger<UpdateBlogSettingsModel>>();
            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<BlogSettings>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<BlogSettings>.Success(new BlogSettings
                {
                    Title = "Blog Title",
                }));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new UpdateBlogSettingsModel(mediaterMock.Object, FeatureManagerMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            var result = await pageModel.OnGetAsync(default);

            result.Should().BeOfType<PageResult>();
            pageModel.FeatureState.IsEnabled.Should().BeFalse();
            pageModel.FeatureState.Message.Should().Be("Disabled!");
        }
    }
}
