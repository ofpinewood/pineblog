# PineBlog <img src="pineblog-logo-256x256.gif" alt="PineBlog" height="44" align="left" />
A blogging engine based on ASP.NET Core MVC Razor Pages and Entity Framework Core.

[![Build Status](https://dev.azure.com/ofpinewood/Of%20Pine%20Wood/_apis/build/status/ofpinewood.pineblog?branchName=master)](https://dev.azure.com/ofpinewood/Of%20Pine%20Wood/_build/latest?definitionId=7&branchName=master)
[![NuGet Badge](https://img.shields.io/nuget/v/Opw.PineBlog.svg)](https://www.nuget.org/packages/Opw.PineBlog/)
[![License: MIT](https://img.shields.io/github/license/ofpinewood/pineblog.svg)](https://github.com/ofpinewood/pineblog/blob/master/LICENSE)

## Opw.PineBlog metapackage
The Opw.PineBlog metapackage includes the following packages.

### Opw.PineBlog.EntityFrameworkCore
The PineBlog data provider that uses Entity Framework Core.

[![NuGet Badge](https://img.shields.io/nuget/v/Opw.PineBlog.EntityFrameworkCore.svg)](https://www.nuget.org/packages/Opw.PineBlog.EntityFrameworkCore/)

### Opw.PineBlog.RazorPages
The PineBlog UI using ASP.NET Core MVC Razor Pages.

[![NuGet Badge](https://img.shields.io/nuget/v/Opw.PineBlog.RazorPages.svg)](https://www.nuget.org/packages/Opw.PineBlog.RazorPages/)

### Opw.PineBlog.Core
The PineBlog core package. This package is a dependency for `Opw.PineBlog.RazorPages` and `Opw.PineBlog.EntityFrameworkCore`.

[![NuGet Badge](https://img.shields.io/nuget/v/Opw.PineBlog.Core.svg)](https://www.nuget.org/packages/Opw.PineBlog.Core/)

## Where can I get it?
You can install [Opw.PineBlog](https://www.nuget.org/packages/Opw.PineBlog/) from the NuGet package manager console:

``` ps
PM> Install-Package Opw.PineBlog
```
## Getting started
[TODO]

## Samples
See [Opw.PineBlog.Sample](/docs/Opw.PineBlog.Sample.md).

## Contributing
We accept fixes and features! Here are some resources to help you get started on how to contribute code or new content.

* [Contributing](https://github.com/ofpinewood/pineblog/blob/master/CONTRIBUTING.md)
* [Code of conduct](https://github.com/ofpinewood/pineblog/blob/master/CODE_OF_CONDUCT.md)

---
Copyright &copy; 2019, [Of Pine Wood](http://ofpinewood.com).
Created by [Peter van den Hout](http://ofpinewood.com).
Released under the terms of the [MIT license](https://github.com/ofpinewood/pineblog/blob/master/LICENSE).