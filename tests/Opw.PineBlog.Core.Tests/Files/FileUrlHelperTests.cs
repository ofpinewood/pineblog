using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Opw.PineBlog.Files
{
    public class FileUrlHelperTests
    {
        private const string FILE_BASE_URL = "http://localhost/azure-blob-storage";
        private const string CONTAINER_NAME = "container-name";

        private readonly FileUrlHelper _fileUrlHelper;

        public FileUrlHelperTests()
        {
            var blogOptionsMock = new Mock<IOptions<PineBlogOptions>>();
            blogOptionsMock.Setup(o => o.Value).Returns(new PineBlogOptions
            {
                FileBaseUrl = FILE_BASE_URL,
                AzureStorageBlobContainerName = CONTAINER_NAME
            });

            _fileUrlHelper = new FileUrlHelper(blogOptionsMock.Object);
        }

        [Fact]
        public void ReplaceBaseUrlWithUrlFormat_Should_ReplaceAllInstances_ForAzureStorageFile()
        {
            var baseUrl = string.Join('/', FILE_BASE_URL, CONTAINER_NAME);
            var s = $"Image 1: {baseUrl}/images/aaa.jpg, image 2: {baseUrl}/images/bbb.jpg.";

            var result = _fileUrlHelper.ReplaceBaseUrlWithUrlFormat(s);

            result.Should().Be("Image 1: %URL%/images/aaa.jpg, image 2: %URL%/images/bbb.jpg.");
        }

        [Fact]
        public void ReplaceUrlFormatWithBaseUrl_Should_ReplaceAllInstances_ForAzureStorageFile()
        {
            var s = "Image 1: %URL%/images/aaa.jpg, image 2: %URL%/images/bbb.jpg.";

            var result = _fileUrlHelper.ReplaceUrlFormatWithBaseUrl(s);

            result.Should().Be($"Image 1: {string.Join('/', FILE_BASE_URL, CONTAINER_NAME, "images/aaa.jpg")}, image 2: {string.Join('/', FILE_BASE_URL, CONTAINER_NAME, "images/bbb.jpg")}.");
        }
    }
}
