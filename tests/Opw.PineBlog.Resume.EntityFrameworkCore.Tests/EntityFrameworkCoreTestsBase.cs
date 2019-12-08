using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Opw.PineBlog.Resume.EntityFrameworkCore
{
    public abstract class EntityFrameworkCoreTestsBase
    {
        protected readonly IServiceCollection Services;

        protected IServiceProvider ServiceProvider => Services.BuildServiceProvider();

        public EntityFrameworkCoreTestsBase()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();

            // create a new in-memory database for each test
            configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value = $"Server=inMemory; Database=pineblog-tests-{DateTime.UtcNow.Ticks};";

            Services = new ServiceCollection();
            Services.AddPineBlogCore(configuration);
            Services.AddPineBlogResumeEntityFrameworkCore(configuration);
        }
    }
}
