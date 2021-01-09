using Opw.PineBlog.EntityFrameworkCore;
using System;
using System.Linq;

namespace Opw.PineBlog.Sample
{
    internal class DatabaseSeed : DbSeedBase
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

            CreatePosts();
        }

        void CreateAuthor(string name, string imagePath)
        {
            if (_dbContext.Authors.Count() > 0) return;

            var email = ApplicationConstants.UserEmail;
            if (_dbContext.Authors.Count(a => a.UserName.Equals(email)) > 0) return;

            _dbContext.Authors.Add(GetAuthor(name, email, imagePath));
            _dbContext.SaveChanges();
        }

        void CreatePosts()
        {
            if (_dbContext.Posts.Count() > 0) return;

            var author = _dbContext.Authors.Single();

            _dbContext.Posts.Add(GetWelcomePost(author.Id));
            _dbContext.Posts.Add(GetMarkdownPost(author.Id));

            for (int i = 1; i < 40; i++)
            {
                _dbContext.Posts.Add(GetPost(i, author.Id));
            }

            _dbContext.SaveChanges();
        }
    }
}
