using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace Opw.PineBlog.GitDb
{
    [Collection(nameof(GitDbCollection))]
    public abstract class GitDbTestsBase
    {
        private readonly GitDbFixture _fixture;

        protected readonly IConfiguration Configuration;
        protected readonly IServiceCollection Services;

        protected IServiceProvider ServiceProvider => Services.BuildServiceProvider();

        public GitDbTestsBase(GitDbFixture fixture)
        {
            _fixture = fixture;

            Configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddPineBlogGitDbConfiguration(reloadOnChange: false)
               .Build();

            Services = new ServiceCollection();
            Services.AddPineBlogCore(Configuration);
            Services.AddPineBlogGitDb(Configuration);

            // add a mock logger for the GitDbSyncService
            Services.AddScoped((_) => new Mock<ILogger<GitDbSyncService>>().Object);
        }
    }
}
