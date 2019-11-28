using Newtonsoft.Json;
using Opw.PineBlog.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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
            var response = await _client.GetAsync("admin/file");
            response.EnsureSuccessStatusCode();
        }
    }
}
