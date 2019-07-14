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
            var email = "pineblog@example.com";
            if (_dbContext.Authors.Count(a => a.UserId.Equals(userId)) > 0) return;

            _dbContext.Authors.Add(new Author
            {
                UserId = userId,
                UserName = email,
                Email = email,
                DisplayName = "John Smith",
                Avatar = "images/avatar-male.png",
                Bio = "It is common knowledge that the consolidation of the mindset cannot be shown to be relevant.This is in contrast to The Affectability Of Determinant Empathy",
            });
            _dbContext.Authors.Add(new Author
            {
                UserId = userId,
                UserName = email,
                Email = email,
                DisplayName = "Mary Smith",
                Avatar = "images/avatar-female.png",
                Bio = "It is common knowledge that the consolidation of the mindset cannot be shown to be relevant.This is in contrast to The Affectability Of Determinant Empathy",
            });

            _dbContext.SaveChanges();
        }

        void CreateBlogPosts()
        {
            if (_dbContext.Posts.Count() > 0) return;

            var index = 0;
            foreach (var author in _dbContext.Authors)
            {
                index++;
                if (_dbContext.Posts.Count(p => p.AuthorId.Equals(author.Id)) > 0) continue;

                _dbContext.Posts.Add(new Post
                {
                    AuthorId = author.Id,
                    Title = "The Affectability Of Determinant Empathy",
                    Slug = $"The Affectability Of Determinant Empathy {index}".ToSlug(),
                    Description = "To be perfectly frank, the target population for any formalization of the proactive dynamic teleology provides a balanced perspective to the strategic organic auto-interruption. We need to be able to rationalize the hierarchical immediate vibrancy. We need to be able to rationalize the marginalised empirical support. One must therefore dedicate resources to the total paralyptic correspondence immediately.",
                    Content = @"So far, the consolidation of the benchmark de-stabilizes any discrete or conceptual configuration mode.

In a strictly mechanistic sense, efforts are already underway in the development of the global business practice. On the other hand, the ball-park figures for the basic definitive rationalization indicates the importance of other systems and the necessity for an elemental change in the adequate timing control.",
                    Categories = "wafflegen",
                    Cover = new Cover
                    {
                        Url = "images/woods.gif",
                    },
                    Published = DateTime.UtcNow.AddDays(-(index * 20) - index)
                });

                _dbContext.Posts.Add(new Post
                {
                    AuthorId = author.Id,
                    Title = "The Element Of Sub-Logical Phenomenon",
                    Slug = $"The Element Of Sub-Logical Phenomenon {index}".ToSlug(),
                    Description = "Without doubt, the assessment of any significant weaknesses in the value added vibrant concept embodies The Element Of Sub-Logical Phenomenon.",
                    Content = @"Whilst it may be true that a proportion of the skill set makes little difference to the philosophy of commonality and standardization. Everything should be done to expedite the two-phase empirical parameter. Everything should be done to expedite the universe of object, one must not lose sight of the fact that a primary interrelationship between system and/or subsystem technologies uniquely legitimises the significance of what should be termed the non-viable expressive program.",
                    Categories = "wafflegen",
                    Published = DateTime.UtcNow.AddDays(-(index * 10) - index)
                });

                _dbContext.Posts.Add(new Post
                {
                    AuthorId = author.Id,
                    Title = "The Disposition Of Non-Referent Discord",
                    Slug = $"The Disposition Of Non-Referent Discord {index}".ToSlug(),
                    Description = "It can be forcibly emphasized that an anticipation of the effects of any homogeneous partnership capitalises on the strengths of the overall game-plan.",
                    Content = @"Without a doubt, any significant enhancements in the purchaser - provider may mean a wide diffusion of the mechanism-independent governing support into the temperamental symbolism. One must therefore dedicate resources to the psychic factor immediately.. So, where to from here? Presumably, The core drivers is generally compatible with the doctrine of the integrated item. Everything should be done to expedite the evolution of precise absorption over a given time limit.",
                    Categories = "wafflegen",
                    Published = DateTime.UtcNow.AddDays(-index - index)
                });
            }

            _dbContext.SaveChanges();
        }
    }
}
