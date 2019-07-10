using Microsoft.Extensions.Options;
using System;
using Microsoft.Extensions.DependencyInjection;
using Opw.EntityFrameworkCore;

namespace Opw.PineBlog.EntityFrameworkCore
{
    public static class ServiceProviderExtensions
    {
        public static void InitializeBlogDatabase(this IServiceProvider serviceProvider)
        {
            var applicationInfoOptions = serviceProvider.GetService<IOptions<ApplicationOptions>>();
            if (applicationInfoOptions.Value.CreateAndSeedDatabases)
            {
                var dbContext = (BlogEntityDbContext)serviceProvider.GetRequiredService<IBlogEntityDbContext>();
                new DbMigrator<BlogEntityDbContext>(dbContext).CreateOrMigrate((context) => new DatabaseSeed(context).Run());
            }
        }
    }
}
