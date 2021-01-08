PineBlog a new blogging engine, light-weight, open source and written in ASP.NET Core MVC Razor Pages, using Entity Framework Core or MongoDb. It is highly
extendable, customizable and easy to integrate in an existing web application.

[![Build Status](https://dev.azure.com/ofpinewood/Of%20Pine%20Wood/_apis/build/status/ofpinewood.pineblog?branchName=master)](https://dev.azure.com/ofpinewood/Of%20Pine%20Wood/_build/latest?definitionId=7&branchName=master)
[![NuGet Badge](https://img.shields.io/nuget/v/Opw.PineBlog.svg)](https://www.nuget.org/packages/Opw.PineBlog/)
[![License: MIT](https://img.shields.io/github/license/ofpinewood/pineblog.svg)](https://github.com/ofpinewood/pineblog/blob/master/LICENSE)

## Features

- Super easy installation, basically add the NuGet package and you're done
- Markdown post editor
- File management
- Light-weight using Razor Pages
- SEO optimized
- Open Graph protocol
- Clean Architecture
- Entity Framework Core, SQL database
- Azure Blob Storage, for file storage
- ..only a blogging engine, nothing else..

### What is not included
Because PineBlog is very light-weight it is not a complete website, it needs to be integrated in an existing web application of you need to create a basic web application for it. There are a few things PineBlog depends on, but that it does not provide.

- Authentication and authorization

> **Note:** The admin pages require that authentication/authorization has been setup in your website, the admin area has  a `AuthorizeFilter` with the default policy set to all pages in that area folder.

## Where can I get it?
You can install the [Opw.PineBlog](https://www.nuget.org/packages/Opw.PineBlog/) metapackage from the console.

``` cmd
> dotnet add package Opw.PineBlog
```

## Getting started
You add the PineBlog services and the RazorPages UI in the Startup.cs of your application.

``` csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddPineBlog(Configuration);
    
    services.AddMvc().AddPineBlogRazorPages();
    // or services.AddMvcCore().AddPineBlogRazorPages();
    ...
}
```

### Configuration
A few properties need to be configured before you can run your web application with PineBlog.

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

## ...more
For more information, please check [PineBlog on GitHub](https://github.com/ofpinewood/pineblog).