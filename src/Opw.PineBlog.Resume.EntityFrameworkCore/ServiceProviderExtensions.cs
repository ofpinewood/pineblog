using Microsoft.Extensions.Options;
using System;
using Microsoft.Extensions.DependencyInjection;
using Opw.EntityFrameworkCore;

namespace Opw.PineBlog.Resume.EntityFrameworkCore
{
    public static class ServiceProviderExtensions
    {
        public static void InitializePineBlogResumeDatabase(this IServiceProvider serviceProvider, Action<ResumeEntityDbContext> seedAction)
        {
            //var blogOptions = serviceProvider.GetService<IOptions<PineBlogOptions>>();
            //if (blogOptions.Value.CreateAndSeedDatabases)
            //{
            //    var dbContext = (BlogEntityDbContext)serviceProvider.GetRequiredService<IBlogEntityDbContext>();
            //    new DbMigrator<BlogEntityDbContext>(dbContext).CreateOrMigrate(seedAction);
            //}
        }
    }
}
