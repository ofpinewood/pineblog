using Opw.PineBlog.Entities;
using Opw.PineBlog.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Text;
using WaffleGenerator;

namespace Opw.PineBlog.Sample
{
    internal abstract class DbSeedBase
    {
        protected Author GetAuthor(string name, string email, string imagePath)
        {
            return new Author
            {
                UserName = email,
                Email = email,
                DisplayName = name,
                Avatar = imagePath,
                Bio = WaffleEngine.Text(1, false),
            };
        }

        protected Post GetPost(int i, Guid authorId)
        {
            var title = WaffleEngine.Title();
            if (title.Length > 160)
                title = title.Substring(0, 160);

            var post = new Post
            {
                AuthorId = authorId,
                Title = title,
                Slug = title.ToPostSlug(),
                Description = WaffleEngine.Text(1, false),
                Published = DateTime.UtcNow.AddDays(-i * 10)
            };

            if (post.Description.Length > 450)
                post.Description = post.Description.Substring(0, 450);

            var content = new StringBuilder();
            content.Append($"## {WaffleEngine.Text(1, true)}");
            content.Append($"{WaffleEngine.Text(1, false)}");

            if (i % 2 == 0)
                content.Append("``` csharp\npublic class {{\n  var myVar = \"Some value\";\n}}\n```\n");
            else
                content.Append("``` yaml\nYAML: YAML Ain't Markup Language\n```\n");

            content.Append($"## {WaffleEngine.Text(1, true)}");
            content.Append($"{WaffleEngine.Text(2, false)}");
            content.Append($"> {WaffleEngine.Text(1, false)}\n");
            content.Append($"{WaffleEngine.Text(1, false)}");

            post.Content = content.ToString();

            if (i % 2 == 0)
            {
                post.CoverUrl = "/images/woods.gif";
                post.CoverCaption = "Battle background for the Misty Woods in the game Shadows of Adam by Tim Wendorf";
                post.CoverLink = "http://pixeljoint.com/pixelart/94359.htm";
                post.Categories = "csharp,waffle,random";
            }
            else
            {
                post.Categories = "yaml,waffle,random";
            }

            return post;
        }

        protected Post GetPostFromFile(Guid authorId)
        {
            var title = "PineBlog an ASP.NET Core blogging engine";

            var assembly = typeof(DatabaseSeed).Assembly;
            var resourceStream = assembly.GetManifestResourceStream("Opw.PineBlog.Sample.Resources.post-pineblog-demo-website.md");
            string content = null;
            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
                content = reader.ReadToEnd();

            var post = new Post
            {
                AuthorId = authorId,
                Title = title,
                Slug = title.ToPostSlug(),
                Description = "PineBlog is a light-weight open source blogging engine written in ASP.NET Core MVC Razor Pages, using Entity Framework Core.",
                Categories = "pineblog,aspnetcore,blog,razor pages,entity framework,github,demo",
                Published = DateTime.UtcNow,
                Content = content
            };

            return post;
        }
    }
}
