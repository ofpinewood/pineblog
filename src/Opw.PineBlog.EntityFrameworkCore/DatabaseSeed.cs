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
                Bio = "You, a bobsleder!? That I'd like to see! I saw you with those two "ladies of the evening" at Elzars. Explain that. That could be 'my' beautiful soul sitting naked on a couch. If I could just learn to play this stupid thing.",
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
                    Title = "This is the worst part. The calm before the battle.",
                    Slug = "This is the worst part The calm before the battle".ToSlug(),
                    Description = "I was having the most wonderful dream. Except you were there, and you were there, and you were there! Humans dating robots is sick. You people wonder why I'm still single? It's 'cause all the fine robot sisters are dating humans!",
                    Content = @"Ah, yes! John Quincy Adding Machine. He struck a chord with the voters when he pledged not to go on a killing spree. This is the worst part. The calm before the battle. Ven ve voke up, ve had zese wodies.

I could if you hadn't turned on the light and shut off my stereo. Do a flip! When the lights go out, it's nobody's business what goes on between two consenting adults. As an interesting side note, as a head without a body, I envy the dead.

Hi, I'm a naughty nurse, and I really need someone to talk to. $9.95 a minute. Bender, hurry! This fuel's expensive! Also, we're dying! Look, everyone wants to be like Germany, but do we really have the pure strength of 'will'?",
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
