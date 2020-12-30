using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Opw.PineBlog.EntityFrameworkCore
{
    public abstract class EntityFrameworkCoreTestsBase
    {
        protected readonly IConfiguration Configuration;
        protected readonly IServiceCollection Services;

        protected IServiceProvider ServiceProvider => Services.BuildServiceProvider();

        public EntityFrameworkCoreTestsBase()
        {
            Configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddPineBlogEntityFrameworkCoreConfiguration(reloadOnChange: false)
               .Build();

            // create a new in-memory database for each test
            Configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value = $"Server=inMemory; Database=pineblog-tests-{Guid.NewGuid()};";

            Services = new ServiceCollection();
            Services.AddPineBlogCore(Configuration);
            Services.AddPineBlogEntityFrameworkCore(Configuration);
        }
    }
}
