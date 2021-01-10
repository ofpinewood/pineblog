# Using MongoDb
When you want to use MongoDb as your database, then you don't use the `Opw.PineBlog` metapackage but you need to install the required packages individually.

- **Opw.PineBlog.MongoDb package**
The PineBlog data provider that uses MongoDb.
[![NuGet Badge](https://img.shields.io/nuget/v/Opw.PineBlog.MongoDb.svg)](https://www.nuget.org/packages/Opw.PineBlog.MongoDb/)

- **Opw.PineBlog.RazorPages package**
The PineBlog UI using ASP.NET Core MVC Razor Pages.
[![NuGet Badge](https://img.shields.io/nuget/v/Opw.PineBlog.RazorPages.svg)](https://www.nuget.org/packages/Opw.PineBlog.RazorPages/)

- **Opw.PineBlog.Core package**
The PineBlog core package. This package is a dependency for `Opw.PineBlog.RazorPages` and `Opw.PineBlog.MongoDb`.
[![NuGet Badge](https://img.shields.io/nuget/v/Opw.PineBlog.Core.svg)](https://www.nuget.org/packages/Opw.PineBlog.Core/)

## Startup
You add the PineBlog services and the Razor Pages UI in the Startup.cs of your application.

``` csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddPineBlogCore(Configuration);
    services.AddPineBlogMongoDb(Configuration);

    services.AddRazorPages().AddPineBlogRazorPages();
    // or services.AddMvcCore().AddPineBlogRazorPages();
    // or services.AddMvc().AddPineBlogRazorPages();
    ...
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{

    // Make sure you enable static file serving
    app.UseStaticFiles();

    ...
    app.UseEndpoints(endpoints =>
    {
        // make sure to add the endpoint mapping for both RazorPages and Controllers
        endpoints.MapRazorPages();
        endpoints.MapControllers();
    });
    ...
}
```

NOTE: Make sure you enable static file serving `app.UseStaticFiles();`, to enable the serving of the css and javascript from the `Opw.PineBlog.RazorPages` packages.

## Configuration
And a few properties need to be configured before you can run your web application with PineBlog.

``` json
{
    "ConnectionStrings": {
        "MongoDbConnection": "inMemory" // MongoDb connection string
    },
    "PineBlogOptions": {
        "Title": "PineBlog",
        "Description": "A blogging engine based on ASP.NET Core MVC Razor Pages and MongoDb",
        "ItemsPerPage": 5,
        "CreateAndSeedDatabases": true,
        "ConnectionStringName": "MongoDbConnection",
        "MongoDbDatabaseName": "pineblog-db",
        "AzureStorageConnectionString": "UseDevelopmentStorage=true",
        "AzureStorageBlobContainerName": "pineblog",
        "FileBaseUrl": "http://127.0.0.1:10000/devstoreaccount1"
    }
}
```

## Blog Settings ConfigurationProvider
To be able to update the blog settings from the admin pages, you need to add the PineBlog `IConfigurationProvider`s to the
`IConfigurationBuilder` in the `Program.cs`. Add `config.AddPineBlogMongoDbConfiguration(reloadOnChange: true);` to `ConfigureAppConfiguration(..)` on the `IWebHostBuilder`.

``` csharp
WebHost.CreateDefaultBuilder(args)
    .UseStartup<Startup>()
    .ConfigureAppConfiguration((hostingContext, config) => {
        config.AddPineBlogMongoDbConfiguration(reloadOnChange: true);
    });
```
