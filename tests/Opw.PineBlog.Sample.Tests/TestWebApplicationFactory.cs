using MediatR.Registration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Opw.PineBlog.EntityFrameworkCore;
using Opw.PineBlog.Files;
using Opw.PineBlog.Sample.Mocks;
using System.Linq;

namespace Opw.PineBlog.Sample
{
    public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
        where TProgram : class
    {
        public IConfigurationRoot Configuration { get; }

        public TestWebApplicationFactory()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                config.AddConfiguration(Configuration);
                config.AddPineBlogEntityFrameworkCoreConfiguration(reloadOnChange: true);
            });

            builder.ConfigureTestServices(services =>
            {
                var mediatRServiceConfiguration = new MediatRServiceConfiguration();
                mediatRServiceConfiguration.RegisterServicesFromAssembly(typeof(TestWebApplicationFactory<>).Assembly);
                ServiceRegistrar.AddMediatRClasses(services, mediatRServiceConfiguration);

                var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IGetPagedFileListQueryFactory));
                services.Remove(serviceDescriptor);

                services.AddTransient<IGetPagedFileListQueryFactory, GetPagedFileListQueryFactoryMock>();
            });
        }
    }
}
