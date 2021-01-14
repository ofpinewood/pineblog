using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Opw.PineBlog.Entities;
using System;
using System.Collections.Generic;
using Xunit;

namespace Opw.PineBlog.MongoDb
{
    [Collection(nameof(MongoDbDatabaseCollection))]
    public abstract class MongoDbTestsBase
    {
        protected readonly IServiceCollection Services;
        protected readonly IConfiguration Configuration;
        protected readonly IMongoCollection<BlogSettings> BlogSettingsCollection;
        protected readonly IMongoCollection<Author> AuthorCollection;
        protected readonly IMongoCollection<Post> PostCollection;

        protected IServiceProvider ServiceProvider => Services.BuildServiceProvider();

        public MongoDbTestsBase(MongoDbDatabaseFixture fixture)
        {
            Configuration = BuildConfiguration(fixture.Runner.ConnectionString);

            Services = new ServiceCollection();
            Services.AddPineBlogCore(Configuration);
            Services.AddPineBlogMongoDb(Configuration);

            var database = ((BlogUnitOfWork)ServiceProvider.GetRequiredService<IBlogUnitOfWork>()).Database;
            BlogSettingsCollection = database.GetCollection<BlogSettings>(CollectionHelper.GetName<BlogSettings>());
            AuthorCollection = database.GetCollection<Author>(CollectionHelper.GetName<Author>());
            PostCollection = database.GetCollection<Post>(CollectionHelper.GetName<Post>());
        }

        protected virtual IConfiguration BuildConfiguration(string connectionString)
        {
            // create a new in-memory database for each test
            var settings = new Dictionary<string, string> {
                { $"ConnectionStrings:MongoDbConnection", connectionString },
                { $"{nameof(PineBlogOptions)}:{nameof(PineBlogOptions.MongoDbDatabaseName)}", $"pineblog-tests-{Guid.NewGuid()}" }
            };

            return new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddInMemoryCollection(settings)
               .Build();
        }
    }
}
