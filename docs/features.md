# Features

- Markdown post editor ([SimpleMDE](https://simplemde.com/))
- File management
- Light-weight using Razor Pages
- SEO optimized
- Open Graph protocol
- Clean Architecture (youtube: [Clean Architecture with ASP.NET Core](https://youtu.be/_lwCVE_XgqI))
- Entity Framework Core, SQL database
- or MongoDb ([MongoDB.Driver](https://www.nuget.org/packages/mongodb.driver))
- Azure Blob Storage, for file storage
- ..only a blogging engine, nothing else..

## Markdown support
[Markdig](https://github.com/xoofx/markdig) is used as the Markdown processor and the following markdown features are enabled by default:

- [Pipe tables](https://github.com/xoofx/markdig/blob/master/src/Markdig.Tests/Specs/PipeTableSpecs.md)
- [Emphasis extras](https://github.com/xoofx/markdig/blob/master/src/Markdig.Tests/Specs/EmphasisExtraSpecs.md)
- [Media links](https://github.com/xoofx/markdig/blob/master/src/Markdig.Tests/Specs/MediaSpecs.md)
- [Generic attributes](https://github.com/xoofx/markdig/blob/master/src/Markdig.Tests/Specs/GenericAttributesSpecs.md)

If you want or require more advanced Markdown features, you can enable those by overriding the `~/Areas/Blog/Pages/Shared/_Post.cshtml` partial (see [source](https://github.com/ofpinewood/pineblog/blob/master/src/Opw.PineBlog.RazorPages/Areas/Blog/Pages/Shared/_Post.cshtml)).

### Markdown examples
How to create a table with styling, using `pipe tables` and `generic attributes`.

```
{.table .table-striped}
|Company|Contact|Country|
|-|-|-|
|Alfreds Futterkiste|Maria Anders|Germany|
|Centro comercial Moctezuma|Francisco Chang|Mexico|
|Ernst Handel|Roland Mendel|Austria|
|Island Trading|Helen Bennett|UK|
```

How to create a blockquote with an bootstrap info-alert style, using `generic attributes`.

```
{.alert .alert-info}
> Normally the dangers inherent in the diverse hardware environment enhances the efficiency of the inductive associative dichotomy on a strictly limited basis.
```

## RSS and ATOM feeds
The RSS and ATOM feeds are exposed through the `FeedController` (from the `Opw.PineBlog.RazorPages` package). They have the following URLs:

- RSS: http://www.example.com/blog/feed/rss ([demo](https://pineblog.azurewebsites.net/blog/feed/rss))
- ATOM: http://www.example.com/blog/feed/atom ([demo](https://pineblog.azurewebsites.net/blog/feed/atom))