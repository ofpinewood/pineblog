# PineBlog <img src="pineblog-logo-256x256.gif" alt="PineBlog" height="44" align="left" />

[![Build Status](https://dev.azure.com/ofpinewood/Of%20Pine%20Wood/_apis/build/status/ofpinewood.pineblog?branchName=master)](https://dev.azure.com/ofpinewood/Of%20Pine%20Wood/_build/latest?definitionId=7&branchName=master)
[![NuGet Badge](https://img.shields.io/nuget/v/Opw.PineBlog.svg)](https://www.nuget.org/packages/Opw.PineBlog/)
[![License: MIT](https://img.shields.io/github/license/ofpinewood/pineblog.svg)](https://github.com/ofpinewood/pineblog/blob/master/LICENSE)

PineBlog is a light-weight blogging engine written in ASP.NET Core MVC Razor Pages, using Entity Framework Core. It is highly extendable, customizable and easy to integrate in an existing web application.

![PineBlog screenshot](docs/screenshot.png)

### Features

- Markdown post editor
- File management
- Light-weight using Razor Pages
- SEO optimized
- Open Graph protocol
- Clean Architecture (youtube: [Clean Architecture with ASP.NET Core](https://youtu.be/_lwCVE_XgqI))
- Entity Framework Core, SQL database
- Azure Blob Storage, for file storage
- ..only a blogging engine, nothing else..

### What is not included
Because PineBlog is very light-weight it is not a complete web application, it needs to be integrated in an existing web application or you need to create a basic web application for it. There are a few things PineBlog depends on, but that it does not provide.

- Authentication and authorization

> **Note:** The admin pages require that authentication/authorization has been setup in your website, the admin area has  a `AuthorizeFilter` with the default policy set to all pages in that area folder.

## Where can I get it?
You can install the [Opw.PineBlog](https://www.nuget.org/packages/Opw.PineBlog/) metapackage from the console.

``` cmd
> dotnet add package Opw.PineBlog
```

The Opw.PineBlog metapackage includes the following packages.

- **Opw.PineBlog.EntityFrameworkCore package**  
The PineBlog data provider that uses Entity Framework Core.  
[![NuGet Badge](https://img.shields.io/nuget/v/Opw.PineBlog.EntityFrameworkCore.svg)](https://www.nuget.org/packages/Opw.PineBlog.EntityFrameworkCore/)

- **Opw.PineBlog.RazorPages package**  
The PineBlog UI using ASP.NET Core MVC Razor Pages.  
[![NuGet Badge](https://img.shields.io/nuget/v/Opw.PineBlog.RazorPages.svg)](https://www.nuget.org/packages/Opw.PineBlog.RazorPages/)

- **Opw.PineBlog.Core package**  
The PineBlog core package. This package is a dependency for `Opw.PineBlog.RazorPages` and `Opw.PineBlog.EntityFrameworkCore`.  
[![NuGet Badge](https://img.shields.io/nuget/v/Opw.PineBlog.Core.svg)](https://www.nuget.org/packages/Opw.PineBlog.Core/)

## Getting started
You add the PineBlog services and the Razor Pages UI in the Startup.cs of your application.

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

### Configuration
And a few properties need to be configured before you can run your web application with PineBlog.

``` json
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=inMemory; Database=pineblog-db;"
    },
    "PineBlogOptions": {
        "Title": "PineBlog",
        "Description": "A blogging engine based on ASP.NET Core MVC Razor Pages and Entity Framework Core",
        "ItemsPerPage": 5,
        "CreateAndSeedDatabases": true,
        "ConnectionStringName": "DefaultConnection",
        "AzureStorageConnectionString": "UseDevelopmentStorage=true",
        "AzureStorageBlobContainerName": "pineblog",
        "FileBaseUrl": "http://127.0.0.1:10000/devstoreaccount1"
    }
}
```

#### Blog Settings ConfigurationProvider
To be able to update the blog settings from the admin pages, you need to add the PineBlog `IConfigurationProvider`s to the
`IConfigurationBuilder` in the `Program.cs`. Add `config.AddPineBlogEntityFrameworkCoreConfiguration(reloadOnChange: true);` to `ConfigureAppConfiguration(..)` on the `IWebHostBuilder`.

``` csharp
WebHost.CreateDefaultBuilder(args)
    .UseStartup<Startup>()
    .ConfigureAppConfiguration((hostingContext, config) => {
        config.AddPineBlogEntityFrameworkCoreConfiguration(reloadOnChange: true);
    });
```

## Documentation
For more information, please check the documentation.

- [Getting started](https://github.com/ofpinewood/pineblog/tree/master/docs/getting-started.md)
- [Customizing the layout](https://github.com/ofpinewood/pineblog/tree/master/docs/custom-layout.md)
- [Features](https://github.com/ofpinewood/pineblog/tree/master/docs/features.md)

For technical background information, check the blog: [ofpinewood.com](https://ofpinewood.com/Blog?category=pineblog).

## Samples
- The [sample project](https://github.com/ofpinewood/pineblog/tree/master/samples/Opw.PineBlog.Sample) contains an example web application with PineBlog.  
- The [NuGet sample project](https://github.com/ofpinewood/pineblog/tree/master/samples/Opw.PineBlog.Sample.NuGet) contains an example web application using just the NuGet packages.

**Please see the code** :nerd_face:

## Demo website
The demo site is a playground to check out PineBlog. You can write and publish posts, upload files and test application before install.
And no worries, it is just a sandbox and will clean itself.

> **Url:** [pineblog.azurewebsites.net](https://pineblog.azurewebsites.net)  
> **Username:** pineblog@example.com  
> **Password:** demo  

## Usage
PineBlog is used on the following website:
- [ofpinewood.com](https://ofpinewood.com)

## Contributing
We accept fixes and features! Here are some resources to help you get started on how to contribute code or new content.

* [Contributing](https://github.com/ofpinewood/pineblog/blob/master/CONTRIBUTING.md)
* [Code of conduct](https://github.com/ofpinewood/pineblog/blob/master/CODE_OF_CONDUCT.md)

---
Copyright &copy; 2019, [Of Pine Wood](http://ofpinewood.com).
Created by [Peter van den Hout](http://ofpinewood.com).
Released under the terms of the [MIT license](https://github.com/ofpinewood/pineblog/blob/master/LICENSE).
