# PineBlog <img src="pineblog-logo-256x256.gif" alt="PineBlog" height="44" align="left" />

[![Build Status](https://dev.azure.com/ofpinewood/Of%20Pine%20Wood/_apis/build/status/ofpinewood.pineblog?branchName=master)](https://dev.azure.com/ofpinewood/Of%20Pine%20Wood/_build/latest?definitionId=7&branchName=master)
[![NuGet Badge](https://img.shields.io/nuget/v/Opw.PineBlog.svg)](https://www.nuget.org/packages/Opw.PineBlog/)
[![License: MIT](https://img.shields.io/github/license/ofpinewood/pineblog.svg)](https://github.com/ofpinewood/pineblog/blob/master/LICENSE)

PineBlog is a light-weight blogging engine written in ASP.NET Core MVC Razor Pages, using Entity Framework Core. It is highly extendable, customizable and easy to integrate in an existing web application.

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
Because PineBlog is very light-weight it is not a complete website, it needs to be integrated in an existing web application of you need to create a basic web application for it. There are a few things PineBlog depends on, but that it does not provide.

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
A few properties need to be configured before you can run your web application with PineBlog.

**Title:** the title of your blog/website.  
**CoverUrl:** the URL for the cover image of your blog, this can be a relative or absolute URL.  
**ConnectionStringName:** this is the name to the connection string used in your application.  
**CreateAndSeedDatabases:** to automatically create and seed the tables for the blog set this property to `true`, if you want to create and seed your database in any other way set this property to `false`.  
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
        "CreateAndSeedDatabases": true,
        "ConnectionStringName": "DefaultConnection",
        "AzureStorageConnectionString": "UseDevelopmentStorage=true",
        "AzureStorageBlobContainerName": "pineblog",
        "FileBaseUrl": "http://127.0.0.1:10000/devstoreaccount1"
    }
}
```

## Blog layout page
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

### Your layout page
PineBlog is dependent on [Bootstrap 4.3](https://getbootstrap.com/) and [Font Awesome 4.7](https://fontawesome.com/v4.7.0/), so make sure to include them in your layout page and add the necessary files to the `wwwroot` of your project (see the sample project for an example).

``` html
<html>
    <head>
        ...
        <environment include="Development">
            <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Open+Sans:400,700|Merriweather:700">
            <link rel="stylesheet" href="~/css/bootstrap.css" />
            <link rel="stylesheet" href="~/css/font-awesome.min.css">
        </environment>
        <environment exclude="Development">
            <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Open+Sans:400,700|Merriweather:700">
            <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css"
                asp-fallback-href="~/css/bootstrap.min.css"
                asp-fallback-test-class="sr-only"
                asp-fallback-test-property="position"
                asp-fallback-test-value="absolute"
                integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T"
                crossorigin="anonymous">
            <link rel="stylesheet" href="~/css/font-awesome.min.css" asp-append-version="true">
        </environment>
        ...
    </head>
    <body>
        ...
        <environment include="Development">
            <script src="~/js/jquery.js"></script>
            <script src="~/js/popper.min.js"></script>
            <script src="~/js/bootstrap.js"></script>
        </environment>
        <environment exclude="Development">
            <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"
                    asp-fallback-src="~/js/jquery.min.js"
                    asp-fallback-test="window.jQuery"
                    integrity="sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8="
                    crossorigin="anonymous">
            </script>
            <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"
                    integrity="sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1"
                    crossorigin="anonymous">
            </script>
            <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"
                    asp-fallback-src="~/js/bootstrap.min.js"
                    asp-fallback-test="window.jQuery && window.jQuery.fn && window.jQuery.fn.modal"
                    integrity="sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM"
                    crossorigin="anonymous">
            </script>
        </environment>
    </body>
</html>
```

### Open Graph protocol
The blog pages include Open Graph `<meta>` tags in the `<head>` of the page. These require that you add the `og: http://ogp.me/ns#` prefix to the `<html>` tag in your pages.

``` html
<html prefix="og: http://ogp.me/ns#">
```

### Overriding the UI
You can override any other Razor view you like by following the same steps as described above for the layout page. For an example have a look at the sample project where we override the footer ([_Footer.cshtml](https://github.com/ofpinewood/pineblog/blob/master/samples/Opw.PineBlog.Sample/Areas/Blog/Pages/Shared/_Footer_.cshtml)).

## Admin layout page
For the **Admin** area layout page do the same as you did for the **Blog** area.

## Samples
The [sample project](https://github.com/ofpinewood/pineblog/tree/master/samples/Opw.PineBlog.Sample) contains an example web application with PineBlog.

**Please see the code** :nerd_face

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
