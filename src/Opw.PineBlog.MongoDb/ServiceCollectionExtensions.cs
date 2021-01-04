using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Opw.PineBlog.Entities;

namespace Opw.PineBlog.MongoDb
{
    /// <summary>
    /// Provides extension methods for the Microsoft.Extensions.DependencyInjection.IServiceCollection interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds PineBlog MongoDb services to the specified services collection.
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <param name="configuration">The application configuration properties.</param>
        /// <returns>The original services object.</returns>
        public static IServiceCollection AddPineBlogMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            // TODO: add check for when someone tries to add multiple databases

            RegisterClassMappings();

            services.AddTransient<IBlogUnitOfWork>(provider =>
            {
                var blogOptions = provider.GetRequiredService<IOptions<PineBlogOptions>>();
                var connectionString = configuration.GetConnectionString(blogOptions.Value.ConnectionStringName);
                var client = new MongoClient(connectionString);

                return new BlogUnitOfWork(client.GetDatabase(blogOptions.Value.MongoDbDatabaseName));
            });

            return services;
        }

        private static void RegisterClassMappings()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(BlogSettings)))
            {
                BsonClassMap.RegisterClassMap<BlogSettings>(m =>
                {
                    m.AutoMap();
                    m.MapIdProperty(bs => bs.Created);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Post)))
            {
                BsonClassMap.RegisterClassMap<Post>(m =>
                {
                    m.AutoMap();
                    m.MapIdProperty(p => p.Id);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Author)))
            {
                BsonClassMap.RegisterClassMap<Author>(m =>
                {
                    m.AutoMap();
                    m.MapIdProperty(a => a.Id);
                });
            }
        }
    }
}
