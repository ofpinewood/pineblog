using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.RazorPages.Areas.Blog.Pages
{
    public class IndexModelTests : RazorPagesTestsBase
    {
        [Fact]
        public async Task OnGetAsync_Should_SetPostListModel()
        {
            var loggerMock = new Mock<ILogger<IndexModel>>();
            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<PostListModel>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<PostListModel>.Success(GetPostListModel()));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new IndexModel(mediaterMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            var result = await pageModel.OnGetAsync(default, 0);

            result.Should().BeOfType<PageResult>();
            pageModel.PostList.Blog.Should().NotBeNull();
            pageModel.PostList.Pager.Should().NotBeNull();
            pageModel.PostList.Posts.Should().NotBeNull();
            pageModel.PostList.Blog.Title.Should().Be("Blog title");
            pageModel.PostList.Pager.Should().NotBeNull();
            pageModel.PostList.Posts.Should().NotBeNull();
            pageModel.Title.Should().Be("Blog title");
        }

        [Fact]
        public async Task OnGetAsync_Should_SetMetadataModel()
        {
            var loggerMock = new Mock<ILogger<IndexModel>>();
            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<PostListModel>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<PostListModel>.Success(GetPostListModel()));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new IndexModel(mediaterMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            var result = await pageModel.OnGetAsync(default, 0);

            result.Should().BeOfType<PageResult>();
            pageModel.Metadata.Title.Should().Be("Blog title");
            pageModel.Metadata.Description.Should().Be("Blog description");
        }

        [Fact]
        public async Task OnGetAsync_Should_SetPageCoverModel_WhenNoFilters()
        {
            var loggerMock = new Mock<ILogger<IndexModel>>();
            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<PostListModel>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<PostListModel>.Success(GetPostListModel()));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new IndexModel(mediaterMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            var result = await pageModel.OnGetAsync(default, 0);

            result.Should().BeOfType<PageResult>();
            pageModel.PageCover.PostListType.Should().Be(PostListType.Blog);
            pageModel.PageCover.Category.Should().BeNull();
        }

        [Fact]
        public async Task OnGetAsync_Should_SetPageCoverModel_WhenFilterOnCategory()
        {
            var loggerMock = new Mock<ILogger<IndexModel>>();
            var postListModel = GetPostListModel();
            postListModel.PostListType = PostListType.Category;
            postListModel.Category = "category";

            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<PostListModel>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<PostListModel>.Success(postListModel));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new IndexModel(mediaterMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            var result = await pageModel.OnGetAsync(default, 0);

            result.Should().BeOfType<PageResult>();
            pageModel.PageCover.PostListType.Should().Be(PostListType.Category);
            pageModel.PageCover.Category.Should().Be("category");
        }

        [Fact]
        public async Task OnGetAsync_Should_SetAbsoluteCoverUrl()
        {
            var loggerMock = new Mock<ILogger<IndexModel>>();

            var postListModel = GetPostListModel();
            postListModel.Blog.CoverUrl = "http://www.example.com/images.jpg";

            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<PostListModel>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<PostListModel>.Success(postListModel));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new IndexModel(mediaterMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            var result = await pageModel.OnGetAsync(default, 0);

            result.Should().BeOfType<PageResult>();
            pageModel.PostList.Blog.CoverUrl.Should().Be("http://www.example.com/images.jpg");
            pageModel.PageCover.CoverUrl.Should().Be("http://www.example.com/images.jpg");
            pageModel.Metadata.Image.Should().Be("http://www.example.com/images.jpg");
        }

        [Fact]
        public async Task OnGetAsync_Should_SetRelativeCoverUrl()
        {
            var loggerMock = new Mock<ILogger<IndexModel>>();

            var postListModel = GetPostListModel();
            postListModel.Blog.CoverUrl = "/images.jpg";

            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<PostListModel>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<PostListModel>.Success(postListModel));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "http";
            httpContext.Request.Host = new HostString("localhost:5001");
            var pageContext = GetPageContext(httpContext);

            var pageModel = new IndexModel(mediaterMock.Object, loggerMock.Object)
            {
                PageContext = pageContext.Item1,
                TempData = GetTempDataDictionary(httpContext),
                Url = new UrlHelper(pageContext.Item2)
            };

            var result = await pageModel.OnGetAsync(default, 0);

            result.Should().BeOfType<PageResult>();
            pageModel.PostList.Blog.CoverUrl.Should().Be("/images.jpg");
            pageModel.PageCover.CoverUrl.Should().Be("/images.jpg");
            pageModel.Metadata.Image.Should().Be("http://localhost:5001/images.jpg");
        }

        private PostListModel GetPostListModel()
        {
            return new PostListModel
            {
                PostListType = PostListType.Blog,
                Blog = new BlogModel(new PineBlogOptions { Title = "Blog title", Description = "Blog description" }),
                Pager = new Pager(0),
                Posts = new List<Post>()
            };
        }
    }
}
