using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Opw.PineBlog.Models;
using FluentAssertions;
using Opw.AspNetCore.Testing.Net.Http;
using System.Net;

namespace Opw.PineBlog.Sample.Controllers
{
    public class FileControllerTests : IClassFixture<TestWebApplicationFactory<Program>>
    {
        private readonly TestWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly AuthenticationContext _authenticationContext;

        public FileControllerTests(TestWebApplicationFactory<Program> factory)
        {
            _authenticationContext = new AuthenticationContext
            {
                EmailKey = "Input.Email",
                EmailValue = "pineblog@example.com",
                PasswordKey = "Input.Password",
                PasswordValue = "demo",
                LoginUri = "Account/login?returnUrl=/admin"
            };

            _factory = factory;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        }

        [Fact]
        public async Task Get_Should_Return1File()
        {
            await _client.EnsureAuthenticationCookieAsync(_authenticationContext);

            var response = await _client.GetAsync("admin/file");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<FileListModel>();

            result.Should().NotBeNull();
            result.Files.Should().HaveCount(1);
        }

        [Fact]
        public async Task Upload_Should_ReturnBadRequest_WhenUploadingLargeFile()
        {
            var file = "Opw.PineBlog.Sample.Resources.large-image.jpg";
            await _client.EnsureAuthenticationCookieAsync(_authenticationContext);

            using (var fileStream = typeof(FileControllerTests).Assembly.GetManifestResourceStream(file))
            using (var content = new StreamContent(fileStream))
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(content, "files", file);
                var response = await _client.PostAsync("admin/file/upload", formData);

                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

                var result = await response.Content.ReadAsStringAsync();

                result.Should().Be("File: \"Opw.PineBlog.Sample.Resources.large-image.jpg\" exceeds 1 MB.");
            }
        }
    }
}
