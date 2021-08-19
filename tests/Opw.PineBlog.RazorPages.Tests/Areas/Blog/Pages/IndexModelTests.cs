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
    // TODO: improve test coverage (for search)
    public class IndexModelTests : RazorPagesTestsBase
    {
        private readonly Mock<ILogger<IndexModel>> _loggerMock;
        private readonly Mock<IOptionsSnapshot<PineBlogOptions>> _optionsMock;

        public IndexModelTests()
        {
            _loggerMock = new Mock<ILogger<IndexModel>>();

            _optionsMock = new Mock<IOptionsSnapshot<PineBlogOptions>>();
            _optionsMock.SetupGet(m => m.Value).Returns(new PineBlogOptions());
        }

        [Fact]
        public async Task OnGetAsync_Should_SetPostListModel()
        {
            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<PostListModel>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<PostListModel>.Success(GetPostListModel()));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new IndexModel(mediaterMock.Object, _optionsMock.Object, FeatureManagerMock.Object, _loggerMock.Object)
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
            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<PostListModel>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<PostListModel>.Success(GetPostListModel()));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new IndexModel(mediaterMock.Object, _optionsMock.Object, FeatureManagerMock.Object, _loggerMock.Object)
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
            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<PostListModel>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<PostListModel>.Success(GetPostListModel()));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new IndexModel(mediaterMock.Object, _optionsMock.Object, FeatureManagerMock.Object, _loggerMock.Object)
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
            var postListModel = GetPostListModel();
            postListModel.PostListType = PostListType.Category;
            postListModel.Category = "category";

            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<PostListModel>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<PostListModel>.Success(postListModel));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new IndexModel(mediaterMock.Object, _optionsMock.Object, FeatureManagerMock.Object, _loggerMock.Object)
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
            var postListModel = GetPostListModel();
            postListModel.Blog.CoverUrl = "http://www.example.com/images.jpg";

            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<PostListModel>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<PostListModel>.Success(postListModel));

            var httpContext = new DefaultHttpContext();
            var pageContext = GetPageContext(httpContext);

            var pageModel = new IndexModel(mediaterMock.Object, _optionsMock.Object, FeatureManagerMock.Object, _loggerMock.Object)
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
            var postListModel = GetPostListModel();
            postListModel.Blog.CoverUrl = "/images.jpg";

            var mediaterMock = new Mock<IMediator>();
            mediaterMock.Setup(m => m.Send(It.IsAny<IRequest<Result<PostListModel>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<PostListModel>.Success(postListModel));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "http";
            httpContext.Request.Host = new HostString("localhost:5001");
            var pageContext = GetPageContext(httpContext);

            var pageModel = new IndexModel(mediaterMock.Object, _optionsMock.Object, FeatureManagerMock.Object, _loggerMock.Object)
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
