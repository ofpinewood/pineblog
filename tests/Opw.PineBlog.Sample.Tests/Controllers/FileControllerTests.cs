using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Opw.PineBlog.Models;
using FluentAssertions;
using Opw.AspNetCore.Testing.Net.Http;
using System.Reflection;

namespace Opw.PineBlog.Sample.Controllers
{
    public class FileControllerTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly AuthenticationContext _authenticationContext;

        public FileControllerTests(TestWebApplicationFactory factory)
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
        public async Task Get_Should_BeFound()
        {
            await _client.EnsureAuthenticationCookieAsync(_authenticationContext);

            var response = await _client.GetAsync("admin/file");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<FileListModel>();

            result.Should().NotBeNull();
            result.Files.Should().HaveCount(1);
        }

        [Theory]
        [InlineData("Opw.PineBlog.Sample.Resources.large-image.jpg")]
        public async Task Upload_Should_UploadFile(string file)
        {
            await _client.EnsureAuthenticationCookieAsync(_authenticationContext);

            using (var fileStream = typeof(FileControllerTests).Assembly.GetManifestResourceStream(file))
            using (var content = new StreamContent(fileStream))
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(content, "files", file);
                var response = await _client.PostAsync("admin/file/upload", formData);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
