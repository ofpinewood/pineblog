using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Opw.PineBlog.GitDb.LibGit2;
using System.Linq;

namespace Opw.PineBlog.GitDb
{
    /// <summary>
    /// Provides extension methods for the Microsoft.Extensions.DependencyInjection.IServiceCollection interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds PineBlog GitDb services to the specified services collection.
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <param name="configuration">The application configuration properties.</param>
        /// <returns>The original services object.</returns>
        public static IServiceCollection AddPineBlogGitDb(this IServiceCollection services, IConfiguration configuration)
        {
            if (((IConfigurationRoot)configuration).Providers.SingleOrDefault(p => p.GetType() == typeof(BlogSettingsConfigurationProvider)) == null)
            {
                throw new ConfigurationException("The PineBlog IConfigurationProvider(s) are not configured, please add \"AddPineBlogGitDbConfiguration\" to the \"ConfigureAppConfiguration\" on the \"IWebHostBuilder\".")
                {
                    HelpLink = "https://github.com/ofpinewood/pineblog/blob/main/docs/getting-started.md#blog-settings-configurationprovider"
                };
            }

            services.Configure<PineBlogGitDbOptions>(configuration.GetSection(nameof(PineBlogGitDbOptions)));

            services.AddTransient<GitDbContext>(provider =>
            {
                var options = provider.GetRequiredService<IOptions<PineBlogGitDbOptions>>();
                return GitDbContext.Create(options.Value);
            });
            services.AddTransient<IBlogUnitOfWork, BlogUnitOfWork>();

            return services;
        }
    }
}
