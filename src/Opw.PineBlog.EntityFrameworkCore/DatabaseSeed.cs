using Opw.PineBlog.Entities;
using System;
using System.Linq;

namespace Opw.PineBlog.EntityFrameworkCore
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
            CreateAuthors();
            CreateBlogPosts();
        }

        void CreateAuthors()
        {
            if (_dbContext.Authors.Count() > 0) return;

            var userId = Guid.Parse("bdab3e4b-e676-4490-9ed5-f7769b4ef232");
            var email = "pineblog@ofpinewood.com";
            if (_dbContext.Authors.Count(a => a.UserId.Equals(userId)) > 0) return;

            _dbContext.Authors.Add(new Author
            {
                UserId = userId,
                UserName = email,
                Email = email,
                DisplayName = "Peter van den Hout",
                Avatar = "blog/images/avatar.jpg",
                Bio = "It is common knowledge that the consolidation of the mindset cannot be shown to be relevant.This is in contrast to The Affectability Of Determinant Empathy",
            });

            _dbContext.SaveChanges();
        }

        void CreateBlogPosts()
        {
            if (_dbContext.Posts.Count() > 0) return;

            foreach (var author in _dbContext.Authors)
            {
                if (_dbContext.Posts.Count(p => p.AuthorId.Equals(author.Id)) > 0) continue;

                _dbContext.Posts.Add(new Post
                {
                    AuthorId = author.Id,
                    Title = "The Affectability Of Determinant Empathy",
                    Slug = "The Affectability Of Determinant Empathy".ToSlug(),
                    Description = "To be perfectly frank, the target population for any formalization of the proactive dynamic teleology provides a balanced perspective to the strategic organic auto-interruption. We need to be able to rationalize the hierarchical immediate vibrancy. We need to be able to rationalize the marginalised empirical support. One must therefore dedicate resources to the total paralyptic correspondence immediately.",
                    Content = @"So far, the consolidation of the benchmark de-stabilizes any discrete or conceptual configuration mode.

In a strictly mechanistic sense, efforts are already underway in the development of the global business practice. On the other hand, the ball-park figures for the basic definitive rationalization indicates the importance of other systems and the necessity for an elemental change in the adequate timing control.",
                    Categories = "config",
                    Cover = new Cover
                    {
                        Url = "blog/images/waterfalls-animated.gif",
                    },
                    Published = DateTime.UtcNow
                });
            }

            _dbContext.SaveChanges();
        }
    }
}
