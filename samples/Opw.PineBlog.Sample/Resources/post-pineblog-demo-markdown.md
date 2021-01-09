## Markdown support
[Markdig](https://github.com/xoofx/markdig) is used as the Markdown processor and the following markdown features are enabled by default:

- [Pipe tables](https://github.com/xoofx/markdig/blob/master/src/Markdig.Tests/Specs/PipeTableSpecs.md)
- [Emphasis extras](https://github.com/xoofx/markdig/blob/master/src/Markdig.Tests/Specs/EmphasisExtraSpecs.md)
- [Media links](https://github.com/xoofx/markdig/blob/master/src/Markdig.Tests/Specs/MediaSpecs.md)
- [Generic attributes](https://github.com/xoofx/markdig/blob/master/src/Markdig.Tests/Specs/GenericAttributesSpecs.md)

## Tables
How to create the following table with styling, using `pipe tables` and `generic attributes`.

{.table .table-striped}
|Company|Contact|Country|
|-|-|-|
|Alfreds Futterkiste|Maria Anders|Germany|
|Centro comercial Moctezuma|Francisco Chang|Mexico|
|Ernst Handel|Roland Mendel|Austria|
|Island Trading|Helen Bennett|UK|

``` markdown
{.table .table-striped}
|Company|Contact|Country|
|-|-|-|
|Alfreds Futterkiste|Maria Anders|Germany|
|Centro comercial Moctezuma|Francisco Chang|Mexico|
|Ernst Handel|Roland Mendel|Austria|
|Island Trading|Helen Bennett|UK|
```

## Using CSS
How to create the following blockquote with an bootstrap info-alert style, using `generic attributes`.

{.alert .alert-info}
> Normally the dangers inherent in the diverse hardware environment enhances the efficiency of the inductive associative dichotomy on a strictly limited basis.

``` markdown
{.alert .alert-info}
> Normally the dangers inherent in the diverse hardware environment enhances the efficiency of the inductive associative dichotomy on a strictly limited basis.
```