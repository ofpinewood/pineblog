using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Opw.PineBlog.MongoDb
{
    public abstract class MongoDbTestsBase
    {
        protected readonly IServiceCollection Services;

        protected IServiceProvider ServiceProvider => Services.BuildServiceProvider();

        public MongoDbTestsBase()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               // TODO: implement PineBlogConfiguration for MongoDb
               //.AddPineBlogConfiguration(reloadOnChange: false)
               .Build();

            Services = new ServiceCollection();
            Services.AddPineBlogCore(configuration);
            Services.AddPineBlogMongoDb(configuration);
        }
    }
}
