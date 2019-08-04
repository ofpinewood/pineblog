using Microsoft.Extensions.DependencyInjection;
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
        /// <param name="connectionString">The connectionString.</param>
        /// <returns>The original services object.</returns>
        public static IServiceCollection AddPineBlogEntityFrameworkCore(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextPool<IBlogEntityDbContext, BlogEntityDbContext>(options => DbContextConfigurer.Configure(options, connectionString));

            return services;
        }
    }
}
