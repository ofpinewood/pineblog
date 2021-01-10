using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;

namespace Opw.PineBlog.RazorPages
{
    /// <summary>
    /// Provides extension methods for the Microsoft.Extensions.DependencyInjection.IMvcCoreBuilder and Microsoft.Extensions.DependencyInjection.IMvcBuilder interfaces.
    /// </summary>
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Adds PineBlog Razor Pages services to the services collection.
        /// </summary>
        /// <param name="builder">The Microsoft.Extensions.DependencyInjection.IMvcCoreBuilder.</param>
        public static IMvcCoreBuilder AddPineBlogRazorPages(this IMvcCoreBuilder builder)
        {
            ConfigureServices(builder.Services);

            builder.AddApplicationPart(typeof(Controllers.FileController).Assembly);
            builder.AddRazorPages(SetRazorPagesOptions);
            builder.AddMvcOptions(options =>
            {
                options.Filters.Add<PineBlogViewDataAsyncPageFilter>();
            });
            builder.AddFluentValidation();

            return builder;
        }

        /// <summary>
        /// Adds PineBlog Razor Pages services to the services collection.
        /// </summary>
        /// <param name="builder">The Microsoft.Extensions.DependencyInjection.IMvcBuilder.</param>
        public static IMvcBuilder AddPineBlogRazorPages(this IMvcBuilder builder)
        {
            ConfigureServices(builder.Services);

            builder.AddApplicationPart(typeof(Controllers.FileController).Assembly);
            builder.AddRazorPagesOptions(SetRazorPagesOptions);
            builder.AddMvcOptions(options =>
            {
                options.Filters.Add<PineBlogViewDataAsyncPageFilter>();
            });
            builder.AddFluentValidation();

            return builder;
        }

        private static void SetRazorPagesOptions(RazorPagesOptions options)
        {
            options.Conventions.AuthorizeAreaFolder("Admin", "/");
            options.Conventions.AddAreaPageRoute("Blog", "/Post", PineBlogConstants.BlogAreaPath + "/{*slug}");
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureOptions(typeof(StaticFilePostConfigureOptions));
            services.Configure<PineBlogOptions>(options =>
            {
                options.PagingUrlPartFormat = "page={0}";
                options.CategoryUrlPartFormat = "category={0}";
                options.SearchQueryUrlPartFormat = "q={0}";

                var version = typeof(StaticFilePostConfigureOptions).Assembly.GetName().Version;
                options.Version = version.ToString();
            });
        }
    }
}
