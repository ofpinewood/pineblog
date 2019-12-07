using Microsoft.Extensions.DependencyInjection;

namespace Opw.PineBlog.Resume.RazorPages
{
    /// <summary>
    /// Provides extension methods for the Microsoft.Extensions.DependencyInjection.IMvcCoreBuilder and Microsoft.Extensions.DependencyInjection.IMvcBuilder interfaces.
    /// </summary>
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Adds PineBlog resume module Razor Pages services to the services collection.
        /// </summary>
        /// <param name="builder">The Microsoft.Extensions.DependencyInjection.IMvcCoreBuilder.</param>
        public static IMvcCoreBuilder AddPineBlogResumeRazorPages(this IMvcCoreBuilder builder)
        {
            ConfigureServices(builder.Services);

            return builder;
        }

        /// <summary>
        /// Adds PineBlog resume module Razor Pages services to the services collection.
        /// </summary>
        /// <param name="builder">The Microsoft.Extensions.DependencyInjection.IMvcBuilder.</param>
        public static IMvcBuilder AddPineBlogResumeRazorPages(this IMvcBuilder builder)
        {
            ConfigureServices(builder.Services);

            return builder;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            
        }
    }
}
