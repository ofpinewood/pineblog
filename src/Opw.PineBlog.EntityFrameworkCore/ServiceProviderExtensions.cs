using Microsoft.Extensions.Options;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Opw.PineBlog.EntityFrameworkCore
{
    public static class ServiceProviderExtensions
    {
        public static void InitializePineBlogDatabase(this IServiceProvider serviceProvider, Action<BlogEntityDbContext> seedAction)
        {
            var blogOptions = serviceProvider.GetService<IOptions<PineBlogOptions>>();
            if (blogOptions.Value.CreateAndSeedDatabases)
            {
                var dbContext = (BlogEntityDbContext)serviceProvider.GetRequiredService<BlogEntityDbContext>();
                new DbMigrator<BlogEntityDbContext>(dbContext).CreateOrMigrate(seedAction);
            }
        }
    }
}
