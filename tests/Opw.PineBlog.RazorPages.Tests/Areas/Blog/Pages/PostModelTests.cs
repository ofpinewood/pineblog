using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Opw.HttpExceptions;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.RazorPages.Areas.Blog.Pages
{
    public class PostModelTests : RazorPagesTestsBase
    {
        [Fact]
        public async Task OnGetAsync_Should_SetPostModel()
        {
            var loggerMock = new Mock<ILogger<PostModel>>();
            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<PineBlog.Models.PostModel>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<PineBlog.Models.PostModel>.Success(GetPostModel()));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new PostModel(mediaterMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            var result = await pageModel.OnGetAsync("slug", default);

            result.Should().BeOfType<PageResult>();
            pageModel.Post.Blog.Title.Should().Be("Blog title");
            pageModel.Post.Post.Title.Should().Be("Post title");
            pageModel.Post.Post.Author.DisplayName.Should().Be("Post author");
            pageModel.Title.Should().Be("Post title");
            pageModel.Metadata.Description.Should().Be("Post description");
            pageModel.Metadata.Author.Should().Be("Post author");
            pageModel.PageCover.Title.Should().Be("Post title");
            pageModel.BlogTitle.Should().Be("Blog title");
        }

        [Fact]
        public async Task OnGetAsync_Should_SetPostModel_WithAbsoluteCoverUrl()
        {
            var loggerMock = new Mock<ILogger<PostModel>>();

            var postModel = GetPostModel();
            postModel.Post.CoverUrl = "http://www.example.com/images.jpg";

            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<PineBlog.Models.PostModel>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<PineBlog.Models.PostModel>.Success(postModel));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new PostModel(mediaterMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            var result = await pageModel.OnGetAsync("slug", default);

            result.Should().BeOfType<PageResult>();
            pageModel.Post.Post.CoverUrl.Should().Be("http://www.example.com/images.jpg");
            pageModel.Metadata.Image.Should().Be("http://www.example.com/images.jpg");
        }

        [Fact]
        public async Task OnGetAsync_Should_SetPostModel_WithRelativeCoverUrl()
        {
            var loggerMock = new Mock<ILogger<PostModel>>();

            var postModel = GetPostModel();
            postModel.Post.CoverUrl = "/images.jpg";

            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<PineBlog.Models.PostModel>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<PineBlog.Models.PostModel>.Success(postModel));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "http";
            httpContext.Request.Host = new HostString("localhost:5001");
            var pageContext = GetPageContext(httpContext);

            var pageModel = new PostModel(mediaterMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            var result = await pageModel.OnGetAsync("slug", default);

            result.Should().BeOfType<PageResult>();
            pageModel.Post.Post.CoverUrl.Should().Be("/images.jpg");
            pageModel.Metadata.Image.Should().Be("http://localhost:5001/images.jpg");
        }

        [Fact]
        public async Task OnGetAsync_Should_ThrowNotFoundException()
        {
            var loggerMock = new Mock<ILogger<PostModel>>();
            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<PineBlog.Models.PostModel>>>(), It.IsAny<CancellationToken>())).Throws<NotFoundException>();

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new PostModel(mediaterMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            Func<Task> action = async () => await pageModel.OnGetAsync("slug", default);

            action.Should().Throw<NotFoundException>();
        }

        private PineBlog.Models.PostModel GetPostModel()
        {
            return new PineBlog.Models.PostModel
            {
                Blog = new BlogModel(new PineBlogOptions { Title = "Blog title" }),
                Post = new Post()
                {
                    Title = "Post title",
                    Description = "Post description",
                    Author = new Author { DisplayName = "Post author" }
                }
            };
        }
    }
}
