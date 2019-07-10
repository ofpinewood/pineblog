using Microsoft.Extensions.DependencyInjection;
using Opw.EntityFrameworkCore;

namespace Opw.PineBlog.EntityFrameworkCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlogEntityFrameworkCore(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<IBlogEntityDbContext, BlogEntityDbContext>(options => DbContextConfigurer.Configure(options, connectionString), ServiceLifetime.Transient);

            return services;
        }
    }
}
