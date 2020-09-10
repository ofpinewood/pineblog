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
        /// <remarks>Adds the PineBlog core and PineBlog Entity Framework Core services.
        /// You can add them separately by using AddPineBlogCore and/or AddPineBlogEntityFrameworkCore.</remarks>
        /// <returns>The original services object.</returns>
        public static IServiceCollection AddPineBlog(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPineBlogCore(configuration);
            services.AddPineBlogEntityFrameworkCore(configuration);

            return services;
        }
    }
}
