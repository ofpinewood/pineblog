using Opw.PineBlog.Entities;
using Opw.PineBlog.EntityFrameworkCore;
using System;
using System.Linq;
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

            var email = "pineblog@example.com";
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

            for(int i = 0; i < 5; i++)
            {
                var title = WaffleEngine.Title();
                var post = new Post
                {
                    AuthorId = author.Id,
                    Title = title,
                    Slug = title.ToSlug(),
                    Description = WaffleEngine.Text(1, false),
                    Content = $"## {WaffleEngine.Text(1, true)}  {WaffleEngine.Text(1, false)}  ### {WaffleEngine.Text(1, true)}  ## {WaffleEngine.Text(1, true)}  {WaffleEngine.Text(2, false)}",
                    Categories = "waffle random",
                    Published = DateTime.UtcNow.AddDays(-i * 10)
                };

                if (i % 2 == 0)
                {
                    post.CoverUrl = "/images/woods.gif";
                    post.CoverCaption = "Battle background for the Misty Woods in the game Shadows of Adam by Tim Wendorf";
                    post.CoverLink = "http://pixeljoint.com/pixelart/94359.htm";
                    post.Content = $"## {WaffleEngine.Text(1, true)}  {WaffleEngine.Text(1, false)}  \n``` csharp\npublic class {{\n  var myVar = \"Some value\";\n}}\n```\n  ## {WaffleEngine.Text(1, true)}  {WaffleEngine.Text(2, false)}";
                }

                _dbContext.Posts.Add(post);
            }

            _dbContext.SaveChanges();
        }
    }
}
