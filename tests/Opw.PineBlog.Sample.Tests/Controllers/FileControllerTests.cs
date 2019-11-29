using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Opw.PineBlog.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Sample.Controllers
{
    public class FileControllerTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public FileControllerTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Get_Should_BeFound()
        {
            await LoginAsync();
            var response = await _client.GetAsync("admin/file");
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("Resources/large-image.jpg")]
        public async Task Upload_Should_UploadFile(string file)
        {
            using (var fileStream = File.OpenRead(file))
            using (var content = new StreamContent(fileStream))
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(content, "files", file);
                var response = await _client.PostAsync("admin/file/upload", formData);
            }
        }

        private async Task LoginAsync()
        {
            var formData = new Dictionary<string, string>();
            formData.Add("Input.Email", "pineblog@example.com");
            formData.Add("Input.Password", "demo");
            formData.Add("__RequestVerificationToken", await EnsureAntiforgeryToken());
            var response = await _client.PostAsync("Account/Login", new FormUrlEncodedContent(formData));
        }

        protected async Task<string> EnsureAntiforgeryToken()
        {
            SetCookieHeaderValue antiforgeryCookie = null;
            var response = await _client.GetAsync("/Account/Login");
            response.EnsureSuccessStatusCode();
            if (response.Headers.TryGetValues("Set-Cookie", out var values))
                antiforgeryCookie = SetCookieHeaderValue.ParseList(values.ToList())
                    .SingleOrDefault(c => c.Name.StartsWith(".AspNetCore.AntiForgery.", StringComparison.InvariantCultureIgnoreCase));

            antiforgeryCookie.Should().NotBeNull();

            _client.DefaultRequestHeaders.Add("Cookie", new CookieHeaderValue(antiforgeryCookie.Name, antiforgeryCookie.Value).ToString());

            var responseHtml = await response.Content.ReadAsStringAsync();

            var antiforgeryFormFieldRegex = new Regex(@"\<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" \/\>");
            var match = antiforgeryFormFieldRegex.Match(responseHtml);
            var antiforgeryToken = match.Success ? match.Groups[1].Captures[0].Value : null;

            antiforgeryToken.Should().NotBeNull();

            return antiforgeryToken;
        }
    }
}
