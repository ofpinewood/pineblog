using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Files;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Posts
{
    public class PostUrlHelperTests
    {
        private const string FILE_BASE_URL = "http://localhost/azure-blob-storage";
        private const string CONTAINER_NAME = "container-name";

        private readonly Post _postWithBaseUrls;
        private readonly Post _postWithUrlFormat;
        private readonly PostUrlHelper _postUrlHelper;

        public PostUrlHelperTests()
        {
            _postWithBaseUrls = new Post
            {
                Content = "content with an url: http://localhost/azure-blob-storage/container-name/pineblog-tests/content-url-1. nice isn't it?",
                CoverUrl = "http://localhost/azure-blob-storage/container-name/pineblog-tests/blog-cover-url",
            };

            _postWithUrlFormat = new Post
            {
                Content = "content with an url: %URL%/pineblog-tests/content-url-1. nice isn't it?",
                CoverUrl = "%URL%/pineblog-tests/blog-cover-url",
            };

            var blogOptionsMock = new Mock<IOptions<PineBlogOptions>>();
            blogOptionsMock.Setup(o => o.Value).Returns(new PineBlogOptions
            {
                FileBaseUrl = FILE_BASE_URL,
                AzureStorageBlobContainerName = CONTAINER_NAME
            });

            var fileUrlHelper = new FileUrlHelper(blogOptionsMock.Object);

            _postUrlHelper = new PostUrlHelper(fileUrlHelper);
        }

        [Fact]
        public void ReplaceUrlFormatWithBaseUrl_Content()
        {
            var result = _postUrlHelper.ReplaceUrlFormatWithBaseUrl(_postWithUrlFormat);

            result.Should().NotBeNull();
            result.Content.Should().Be("content with an url: http://localhost/azure-blob-storage/container-name/pineblog-tests/content-url-1. nice isn't it?");
        }

        [Fact]
        public void ReplaceUrlFormatWithBaseUrl_CoverUrl()
        {
            var result = _postUrlHelper.ReplaceUrlFormatWithBaseUrl(_postWithUrlFormat);

            result.Should().NotBeNull();
            result.CoverUrl.Should().Be("http://localhost/azure-blob-storage/container-name/pineblog-tests/blog-cover-url");
        }

        [Fact]
        public void ReplaceBaseUrlWithUrlFormat_Content()
        {
            var result = _postUrlHelper.ReplaceBaseUrlWithUrlFormat(_postWithBaseUrls);

            result.Should().NotBeNull();
            result.Content.Should().Be("content with an url: %URL%/pineblog-tests/content-url-1. nice isn't it?");
        }

        [Fact]
        public void ReplaceBaseUrlWithUrlFormat_CoverUrl()
        {
            var result = _postUrlHelper.ReplaceBaseUrlWithUrlFormat(_postWithBaseUrls);

            result.Should().NotBeNull();
            result.CoverUrl.Should().Be("%URL%/pineblog-tests/blog-cover-url");
        }
    }
}
