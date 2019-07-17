using FluentValidation.AspNetCore;
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
        /// <param name="mvcCoreBuilder">The Microsoft.Extensions.DependencyInjection.IMvcCoreBuilder.</param>
        /// <returns>The original services object.</returns>
        public static IServiceCollection AddPineBlogRazorPages(this IServiceCollection services, IMvcCoreBuilder mvcCoreBuilder)
        {
            services.ConfigureOptions(typeof(StaticFilePostConfigureOptions));

            mvcCoreBuilder.AddRazorPages(option =>
            {
                option.Conventions.AuthorizeAreaFolder("Admin", "/");
                option.Conventions.AddAreaPageRoute("Blog", "/Post", "blog/{*slug}");
            });
            services.AddMvcCore()
                .AddRazorPages(option =>
                {
                    option.Conventions.AuthorizeAreaFolder("Admin", "/");
                    option.Conventions.AddAreaPageRoute("Blog", "/Post", "blog/{*slug}");
                })
                .AddFluentValidation();

            services.Configure<BlogOptions>(options => options.PagingUrlPartFormat = "page={0}");

            return services;
        }
    }
}
