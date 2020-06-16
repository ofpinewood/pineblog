using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using MongoDB.Driver;
using Opw.PineBlog.Entities;
using System;

namespace Opw.PineBlog.MongoDb
{
    public abstract class MongoDbTestsBase
    {
        private static MongoDbRunner _runner;

        protected readonly IServiceCollection Services;
        protected readonly IMongoCollection<BlogSettings> BlogSettingsCollection;
        protected readonly IMongoCollection<Author> AuthorCollection;
        protected readonly IMongoCollection<Post> PostCollection;

        protected IServiceProvider ServiceProvider => Services.BuildServiceProvider();

        public MongoDbTestsBase()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               // TODO: implement PineBlogConfiguration for MongoDb
               //.AddPineBlogConfiguration(reloadOnChange: false)
               .Build();

            _runner = MongoDbRunner.Start();

            // create a new in-memory database for each test
            configuration.GetSection("ConnectionStrings").GetSection("MongoDbConnection").Value = _runner.ConnectionString;
            configuration.GetSection("PineBlogOptions").GetSection("MongoDbDatabaseName").Value = $"pineblog-tests-{Guid.NewGuid()}";

            Services = new ServiceCollection();
            Services.AddPineBlogCore(configuration);
            Services.AddPineBlogMongoDb(configuration);

            var database = ((BlogUnitOfWork)ServiceProvider.GetRequiredService<IBlogUnitOfWork>()).Database;
            BlogSettingsCollection = database.GetCollection<BlogSettings>(nameof(BlogSettings));
            AuthorCollection = database.GetCollection<Author>($"{nameof(Author)}s");
            PostCollection = database.GetCollection<Post>($"{nameof(Post)}s");
        }
    }
}
