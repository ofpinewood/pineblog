using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Opw.PineBlog.EntityFrameworkCore
{
    public abstract class EntityFrameworkCoreTestsBase
    {
        protected readonly IServiceCollection Services;

        protected IServiceProvider ServiceProvider => Services.BuildServiceProvider();

        public EntityFrameworkCoreTestsBase()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddPineBlogConfiguration(reloadOnChange: false)
               .Build();

            // create a new in-memory database for each test
            configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value = $"Server=inMemory; Database=pineblog-tests-{Guid.NewGuid()};";

            Services = new ServiceCollection();
            Services.AddPineBlogCore(configuration);
            Services.AddPineBlogEntityFrameworkCore(configuration);
        }
    }
}
