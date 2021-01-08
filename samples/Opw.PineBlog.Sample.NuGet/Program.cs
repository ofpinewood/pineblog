using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Opw.PineBlog.EntityFrameworkCore;
using Opw.PineBlog.MongoDb;
using System;
using System.Collections.Generic;

namespace Opw.PineBlog.Sample.NuGet
{
    public class Program
    {
        private static readonly MongoDbInMemoryRunner _mongoDbRunner = new MongoDbInMemoryRunner();

        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var config = serviceProvider.GetRequiredService<IConfiguration>();

                try
                {
                    if (config.GetValue<string>("PineBlogDataSource") == "MongoDb")
                    {
                        // using MongoDb
                        serviceProvider.InitializePineBlogMongoDb((database) => new MongoDbSeed(database).Run());
                    }
                    else
                    {
                        // using EntityFrameworkCore
                        serviceProvider.InitializePineBlogDatabase((context) => new DatabaseSeed(context).Run());
                    }

                    //var dbContext = serviceProvider.GetRequiredService<BlogEntityDbContext>();
                    //new DatabaseSeed(dbContext).Run();
                }
                catch (Exception ex)
                {
                    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((_, config) =>
                {
                    var configuration = config.Build();
                    if (configuration.GetValue<string>("PineBlogDataSource") == "MongoDb")
                    {
                        // override the ConnectionStringName appsetting to point to the MongoDbConnection
                        var settings = new Dictionary<string, string> { { "PineBlogOptions:ConnectionStringName", "MongoDbConnection" } };
                        config.AddInMemoryCollection(settings);

                        // initializes an in-memory MongoDbRunner when needed
                        _mongoDbRunner.Initialize(config);

                        // using MongoDb as the datasource
                        config.AddPineBlogMongoDbConfiguration(reloadOnChange: true);
                    }
                    else
                    {
                        // using EntityFrameworkCore, the default
                        config.AddPineBlogEntityFrameworkCoreConfiguration(reloadOnChange: true);
                    }
                })
                .ConfigureLogging((hostingContext, builder) =>
                {
                    builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    builder.AddConsole();
                });
    }
}
