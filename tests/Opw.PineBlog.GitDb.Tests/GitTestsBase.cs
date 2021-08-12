using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Opw.PineBlog.GitDb
{
    public abstract class GitTestsBase
    {
        protected readonly IConfiguration Configuration;
        protected readonly IServiceCollection Services;

        protected IServiceProvider ServiceProvider => Services.BuildServiceProvider();

        public GitTestsBase()
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
