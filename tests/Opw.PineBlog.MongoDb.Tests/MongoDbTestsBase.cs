using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using MongoDB.Driver;
using Opw.PineBlog.Entities;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Opw.PineBlog.MongoDb
{
    public abstract class MongoDbTestsBase : IDisposable
    {
        private static MongoDbRunner _runner;

        protected readonly IServiceCollection Services;
        protected readonly IConfiguration Configuration;
        protected readonly IMongoCollection<BlogSettings> BlogSettingsCollection;
        protected readonly IMongoCollection<Author> AuthorCollection;
        protected readonly IMongoCollection<Post> PostCollection;

        protected IServiceProvider ServiceProvider => Services.BuildServiceProvider();

        public MongoDbTestsBase()
        {
            _runner = MongoDbRunner.Start(singleNodeReplSet: true);
            Configuration = BuildConfiguration(_runner.ConnectionString);

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            // always try to dispose, even if disposing=false
            if (_runner != null)
            {
                // wait a little to prevent timeouts
                Thread.Sleep(500);
                try { _runner?.Dispose(); } catch { }
                _runner = null;
                GC.Collect();
            }
        }
    }
}
