# Customizing the layout
For the **Blog** area you need to override the `_Layout.cshtml` for the pages, to do this create a new `_Layout.cshtml` page in the
`~/Areas/Blog/Pages/Shared` folder. This will make the blog pages use that layout page instead of the one included in the `Opw.PineBlog.RazorPages` package.
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

## Your layout page
PineBlog is dependent on [Bootstrap 4.3](https://getbootstrap.com/) and [Font Awesome 4.7](https://fontawesome.com/v4.7.0/), so make sure to include
them in your layout page and add the necessary files to the `wwwroot` of your project (see the sample project for an example).

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

## Open Graph protocol
The blog pages include Open Graph `<meta>` tags in the `<head>` of the page. These require that you add the `og: http://ogp.me/ns#` prefix to the `<html>` tag in your pages.

``` html
<html prefix="og: http://ogp.me/ns#">
```

## Overriding the UI
You can override any other Razor view you like by following the same steps as described above for the layout page.
For an example have a look at the sample project where we override the footer
([_Footer.cshtml](https://github.com/ofpinewood/pineblog/blob/master/samples/Opw.PineBlog.Sample/Areas/Blog/Pages/Shared/_Footer_.cshtml)).

# Admin layout
For the **Admin** area layout page do the same as you did for the **Blog** area.

## Client-side validation
To enable client-side validation you need to add `jquery.validate` or a client-side validation library of your choice.

You need to override the `_ValidationScriptsPartial.cshtml` in the `Admin` area, to do this create a new `_ValidationScriptsPartial.cshtml` page
in the `~/Areas/Admin/Pages/Shared` folder. For an example have a look at the sample project.

Example `_ValidationScriptsPartial.cshtml` partial code:
``` html
<environment include="Development">
    <script src="~/js/jquery.validate.js"></script>
    <script src="~/js/jquery.validate.unobtrusive.js"></script>
</environment>
<environment exclude="Development">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.17.0/jquery.validate.min.js"
            asp-fallback-src="~/js/jquery.validate.min.js"
            asp-fallback-test="window.jQuery && window.jQuery.validator"
            crossorigin="anonymous"
            integrity="sha256-F6h55Qw6sweK+t7SiOJX+2bpSAa3b/fnlrVCJvmEj1A=">
    </script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validation-unobtrusive/3.2.11/jquery.validate.unobtrusive.min.js"
            asp-fallback-src="~/js/jquery.validate.unobtrusive.min.js"
            asp-fallback-test="window.jQuery && window.jQuery.validator && window.jQuery.validator.unobtrusive"
            crossorigin="anonymous"
            integrity="sha256-9GycpJnliUjJDVDqP0UEu/bsm9U+3dnQUH8+3W10vkY=">
    </script>
</environment>
```