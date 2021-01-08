using Microsoft.Extensions.Options;
using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Opw.PineBlog.MongoDb
{
    public static class ServiceProviderExtensions
    {
        public static void InitializePineBlogMongoDb(this IServiceProvider serviceProvider, Action<IMongoDatabase> seedAction)
        {
            var blogOptions = serviceProvider.GetService<IOptions<PineBlogOptions>>();
            if (blogOptions.Value.CreateAndSeedDatabases)
            {
                var database = serviceProvider.GetRequiredService<IMongoDatabase>();
                seedAction(database);
            }
        }
    }
}
