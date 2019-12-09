using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Opw.PineBlog.Resume
{
    /// <summary>
    /// Provides extension methods for the Microsoft.Extensions.DependencyInjection.IServiceCollection interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds PineBlog:Resume services to the specified services collection.
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <param name="configuration">The application configuration properties.</param>
        /// <returns>The original services object.</returns>
        public static IServiceCollection AddPineBlogResume(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddPineBlogResumeCore(configuration);
            //services.AddPineBlogResumeEntityFrameworkCore(configuration);

            return services;
        }
    }
}
