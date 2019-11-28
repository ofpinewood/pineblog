using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Opw.PineBlog.EntityFrameworkCore;

namespace Opw.PineBlog.Sample
{
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        public IConfigurationRoot Configuration { get; }

        public TestWebApplicationFactory()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return new WebHostBuilder()
                .UseConfiguration(Configuration)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((_, config) => config.AddPineBlogConfiguration(reloadOnChange: true));
        }

        //public new HttpClient CreateClient()
        //{
        //    var baseAddress = new Uri($"http://localhost/");
        //    return CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = baseAddress });
        //}
    }
}
