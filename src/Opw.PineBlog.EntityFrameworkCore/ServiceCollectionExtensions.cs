using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Opw.EntityFrameworkCore;

namespace Opw.PineBlog.EntityFrameworkCore
{
    /// <summary>
    /// Provides extension methods for the Microsoft.Extensions.DependencyInjection.IServiceCollection interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds PineBlog Entity Framework Core services to the specified services collection.
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <param name="configuration">The application configuration properties.</param>
        /// <returns>The original services object.</returns>
        public static IServiceCollection AddPineBlogEntityFrameworkCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<IBlogEntityDbContext, BlogEntityDbContext>((provider, options) =>
            {
                var blogOptions = provider.GetRequiredService<IOptions<PineBlogOptions>>();
                var connectionString = configuration.GetConnectionString(blogOptions.Value.ConnectionStringName);
                DbContextConfigurer.Configure(options, connectionString);
            });

            return services;
        }
    }
}
