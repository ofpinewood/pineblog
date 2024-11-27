using MediatR.Registration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.EntityFrameworkCore;
using Opw.PineBlog.Files;
using Opw.PineBlog.Posts;
using Opw.PineBlog.Sample.Mocks;
using System.Linq;

namespace Opw.PineBlog.Sample
{
    public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
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
                .UseStartup<TStartup>()
                .ConfigureAppConfiguration((_, config) => config.AddPineBlogEntityFrameworkCoreConfiguration(reloadOnChange: true));
        }

        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var mediatRServiceConfiguration = new MediatRServiceConfiguration();
                mediatRServiceConfiguration.RegisterServicesFromAssembly(typeof(TestWebApplicationFactory<>).Assembly);
                ServiceRegistrar.AddMediatRClasses(services, mediatRServiceConfiguration);

                var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IGetPagedFileListQueryFactory));
                services.Remove(serviceDescriptor);

                services.AddTransient<IGetPagedFileListQueryFactory, GetPagedFileListQueryFactoryMock>();
            });
            return base.CreateServer(builder);
        }
    }
}
