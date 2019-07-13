using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Opw.PineBlog.EntityFrameworkCore;

namespace Opw.PineBlog
{
    /// <summary>
    /// Provides extension methods for the Microsoft.Extensions.DependencyInjection.IServiceCollection interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds PineBlog services to the specified services collection.
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <param name="configuration">The application configuration properties.</param>
        /// <param name="connectionString">The connectionString.</param>
        /// <returns>The original services object.</returns>
        public static IServiceCollection AddPineBlog(this IServiceCollection services, IConfiguration configuration, string connectionString)
        {
            services.AddPineBlogCore(configuration);
            services.AddPineBlogEntityFrameworkCore(connectionString);
            services.AddPineBlogRazorPages();

            return services;
        }
    }
}
