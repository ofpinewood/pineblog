using Microsoft.Extensions.Options;
using System;
using Microsoft.Extensions.DependencyInjection;
using Opw.EntityFrameworkCore;
using Opw.PineBlog.EntityFrameworkCore;

namespace Opw.PineBlog.Sample
{
    public static class ServiceProviderExtensions
    {
        public static void InitializePineBlogDatabase(this IServiceProvider serviceProvider)
        {
            var blogOptions = serviceProvider.GetService<IOptions<PineBlogOptions>>();
            if (blogOptions.Value.CreateAndSeedDatabases)
            {
                var dbContext = (BlogEntityDbContext)serviceProvider.GetRequiredService<IBlogEntityDbContext>();
                new DbMigrator<BlogEntityDbContext>(dbContext).CreateOrMigrate((context) => new DatabaseSeed(context).Run());
            }
        }
    }
}
