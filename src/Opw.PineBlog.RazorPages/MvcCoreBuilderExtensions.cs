using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Opw.PineBlog
{
    /// <summary>
    /// Provides extension methods for the Microsoft.Extensions.DependencyInjection.IMvcCoreBuilder and Microsoft.Extensions.DependencyInjection.IMvcBuilder interfaces.
    /// </summary>
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Adds PineBlog Razor Pages services to the specified services collection.
        /// </summary>
        /// <param name="builder">The Microsoft.Extensions.DependencyInjection.IMvcCoreBuilder.</param>
        /// <returns>The original services object.</returns>
        public static IMvcCoreBuilder AddPineBlogRazorPages(this IMvcCoreBuilder builder)
        {
            ConfigureServices(builder.Services);

            builder.AddRazorPages(option =>
            {
                option.Conventions.AuthorizeAreaFolder("Admin", "/");
                option.Conventions.AddAreaPageRoute("Blog", "/Post", "blog/{*slug}");
            });
            builder.AddFluentValidation();

            return builder;
        }

        /// <summary>
        /// Adds PineBlog Razor Pages services to the specified services collection.
        /// </summary>
        /// <param name="builder">The Microsoft.Extensions.DependencyInjection.IMvcBuilder.</param>
        /// <returns>The original services object.</returns>
        public static IMvcBuilder AddPineBlogRazorPages(this IMvcBuilder builder)
        {
            ConfigureServices(builder.Services);

            builder.AddRazorPagesOptions(option =>
            {
                option.Conventions.AuthorizeAreaFolder("Admin", "/");
                option.Conventions.AddAreaPageRoute("Blog", "/Post", "blog/{*slug}");
            });
            builder.AddFluentValidation();

            return builder;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureOptions(typeof(StaticFilePostConfigureOptions));
            services.Configure<PineBlogOptions>(options => options.PagingUrlPartFormat = "page={0}");
        }
    }
}