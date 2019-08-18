# PineBlog <img src="pineblog-logo-256x256.gif" alt="PineBlog" height="44" align="left" />

[![Build Status](https://dev.azure.com/ofpinewood/Of%20Pine%20Wood/_apis/build/status/ofpinewood.pineblog?branchName=master)](https://dev.azure.com/ofpinewood/Of%20Pine%20Wood/_build/latest?definitionId=7&branchName=master)
[![NuGet Badge](https://img.shields.io/nuget/v/Opw.PineBlog.svg)](https://www.nuget.org/packages/Opw.PineBlog/)
[![License: MIT](https://img.shields.io/github/license/ofpinewood/pineblog.svg)](https://github.com/ofpinewood/pineblog/blob/master/LICENSE)

A blogging engine based on ASP.NET Core MVC Razor Pages and Entity Framework Core.

- clean architecture
- razor pages
- only a blogging engine

### What is not included
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
Add the PineBlog services and the Razor Pages UI in the Startup.cs of your application.

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

Appsettings.

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
        "CreateAndSeedDatabases": true,
        "ConnectionStringName": "DefaultConnection",
        "AzureStorageConnectionString": "UseDevelopmentStorage=true",
        "AzureStorageBlobContainerName": "pineblog",
        "FileBaseUrl": "http://127.0.0.1:10000/devstoreaccount1"
    }
}
```

### Open Graph protocol
The blog pages include Open Graph `<meta>` tags in the `<head>` of the page. These require that you add the `og: http://ogp.me/ns#` prefix to the `<html>` tag in your pages.

``` html
<html prefix="og: http://ogp.me/ns#">
```

### Blog layout page
For the **Blog** area you need to override the `_Layout.cshtml` for the pages, to do this create a new `_Layout.cshtml` page in the `Areas/Blog/Shared` folder. This will make the blog pages use that layout page instead of the one included in the `Opw.PineBlog.RazorPages` package. 
In the new page you can set the layout page of your website. Make sure to add the `head` and `script` sections.

``` csharp
@{
    Layout = "~/Pages/Shared/_Layout.cshtml";
}
@section head {
    @RenderSection("head", required: false)
}
@section scripts {
    @RenderSection("scripts", required: false)
}
@RenderBody()
```

You can override any other Razor view you like by following the same steps as described above for the layout page. For an example have a look at the sample project where we override the footer ([_Footer.cshtml](https://github.com/ofpinewood/pineblog/blob/master/samples/Opw.PineBlog.Sample/Areas/Blog/Pages/Shared/_Footer_.cshtml)).

### Admin layout page
For the **Admin** area layout page do the same as you did for the **Blog** area.


## Samples
See [Opw.PineBlog.Sample](/docs/Opw.PineBlog.Sample.md).

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
