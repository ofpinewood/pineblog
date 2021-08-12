using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Opw.PineBlog.GitDb
{
    public abstract class GitDbTestsBase
    {
        protected readonly IConfiguration Configuration;
        protected readonly IServiceCollection Services;

        protected IServiceProvider ServiceProvider => Services.BuildServiceProvider();

        public GitDbTestsBase()
        {
            Configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddPineBlogGitDbConfiguration(reloadOnChange: false)
               .Build();

            Services = new ServiceCollection();
            Services.AddPineBlogCore(Configuration);
            Services.AddPineBlogGitDb(Configuration);
        }
    }
}
