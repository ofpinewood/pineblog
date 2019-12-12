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
            var resumeOptions = serviceProvider.GetService<IOptions<PineBlogResumeOptions>>();
            if (resumeOptions.Value.CreateAndSeedDatabases)
            {
                var dbContext = (ResumeEntityDbContext)serviceProvider.GetRequiredService<IResumeEntityDbContext>();
                new DbMigrator<ResumeEntityDbContext>(dbContext).CreateOrMigrate(seedAction);
            }
        }
    }
}
