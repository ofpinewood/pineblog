# Getting started
Add the PineBlog services and the Razor Pages UI in the Startup.cs of your application.

``` csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddPineBlog(Configuration);

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

See [Customizing the layout](https://github.com/ofpinewood/pineblog/tree/master/docs/custom-layout.md) on how to setup the layout pages, css and javascript.

## Configuration
A few properties need to be configured before you can run your web application with PineBlog.

**Title:** the title of your blog/website.
**CoverUrl:** the URL for the cover image of your blog, this can be a relative or absolute URL.
**ConnectionStringName:** this is the name to the connection string used in your application.
**CreateAndSeedDatabases:** to automatically create and seed the tables for the blog set this property to `true`, if you want to create and seed your
database in any other way set this property to `false`.
**AzureStorageConnectionString:** your Azure Blog Storage connection string.
**AzureStorageBlobContainerName:** the name of the blob container to use for file storage.
**FileBaseUrl:** the base URL for the files, this should be the URL for your Azure Blob Storage, e.g. `https://<storage-account>.blob.core.windows.net`.

The rest of the properties are optional and will be set with default values if you don't specify them.

``` json
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=inMemory; Database=pineblog-db;"
    },
    "PineBlogOptions": {
        "Title": "PineBlog",
        "Description": "A blogging engine based on ASP.NET Core MVC Razor Pages and Entity Framework Core",
        "CoverUrl": "/images/woods.gif",
        "CoverCaption": "Battle background for the Misty Woods in the game Shadows of Adam by Tim Wendorf",
        "CoverLink": "http://pixeljoint.com/pixelart/94359.htm",
        "ItemsPerPage": 5,
        "EnableSearch": true, // default is true
        "CreateAndSeedDatabases": true,
        "ConnectionStringName": "DefaultConnection",
        "AzureStorageConnectionString": "UseDevelopmentStorage=true",
        "AzureStorageBlobContainerName": "pineblog",
        "FileBaseUrl": "http://127.0.0.1:10000/devstoreaccount1"
    }
}
```

### Blog Settings ConfigurationProvider
To be able to update the blog settings from the admin pages, you need to add the PineBlog `IConfigurationProvider`s to the
`IConfigurationBuilder` in the `Program.cs`. Add `config.AddPineBlogEntityFrameworkCoreConfiguration(reloadOnChange: true);` to `ConfigureAppConfiguration(..)` on the `IWebHostBuilder`.

``` csharp
WebHost.CreateDefaultBuilder(args)
    .UseStartup<Startup>()
    .ConfigureAppConfiguration((hostingContext, config) => {
        config.AddPineBlogEntityFrameworkCoreConfiguration(reloadOnChange: true);
    });
```