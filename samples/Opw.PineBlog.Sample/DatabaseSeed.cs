using Opw.PineBlog.Entities;
using Opw.PineBlog.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Text;
using WaffleGenerator;

namespace Opw.PineBlog.Sample
{
    internal class DatabaseSeed
    {
        private readonly BlogEntityDbContext _dbContext;

        public DatabaseSeed(BlogEntityDbContext context)
        {
            _dbContext = context;
        }

        public void Run()
        {
            if (DateTime.UtcNow.Day % 2 == 0)
                CreateAuthor("John Smith", "images/avatar-male.png");
            else
                CreateAuthor("Mary Smith", "images/avatar-female.png");

            CreateBlogPosts();
        }

        void CreateAuthor(string name, string imagePath)
        {
            if (_dbContext.Authors.Count() > 0) return;

            var email = ApplicationConstants.UserEmail;
            if (_dbContext.Authors.Count(a => a.UserName.Equals(email)) > 0) return;

            _dbContext.Authors.Add(new Author
            {
                UserName = email,
                Email = email,
                DisplayName = name,
                Avatar = imagePath,
                Bio = WaffleEngine.Text(1, false),
            });

            _dbContext.SaveChanges();
        }

        void CreateBlogPosts()
        {
            if (_dbContext.Posts.Count() > 0) return;

            var author = _dbContext.Authors.Single();

            CreateBlogPostFromFile(author);

            for(int i = 1; i < 4; i++)
            {
                var title = WaffleEngine.Title();
                var post = new Post
                {
                    AuthorId = author.Id,
                    Title = title,
                    Slug = title.ToPostSlug(),
                    Description = WaffleEngine.Text(1, false),
                    Published = DateTime.UtcNow.AddDays(-i * 10)
                };

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

                _dbContext.Posts.Add(post);
            }

            _dbContext.SaveChanges();
        }

        void CreateBlogPostFromFile(Author author)
        {
            var title = "PineBlog an ASP.NET Core blogging engine";

            var assembly = typeof(DatabaseSeed).Assembly;
            var resourceStream = assembly.GetManifestResourceStream("Opw.PineBlog.Sample.Resources.post-pineblog-demo-website.md");
            string content = null;
            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
                content = reader.ReadToEnd();

            var post = new Post
            {
                AuthorId = author.Id,
                Title = title,
                Slug = title.ToPostSlug(),
                Description = "PineBlog is a light-weight open source blogging engine written in ASP.NET Core MVC Razor Pages, using Entity Framework Core.",
                Categories = "pineblog,aspnetcore,blog,razor pages,entity framework,github,demo",
                Published = DateTime.UtcNow,
                Content = content
            };

            _dbContext.Posts.Add(post);
        }
    }
}
