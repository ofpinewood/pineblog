using Microsoft.Extensions.DependencyInjection;

namespace Opw.PineBlog
{
    /// <summary>
    /// Provides extension methods for the Microsoft.Extensions.DependencyInjection.IServiceCollection interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds PineBlog Razor Pages services to the specified services collection.
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <returns>The original services object.</returns>
        public static IServiceCollection AddPineBlogRazorPages(this IServiceCollection services)
        {
            services.ConfigureOptions(typeof(StaticFilePostConfigureOptions));

            return services;
        }
    }
}
