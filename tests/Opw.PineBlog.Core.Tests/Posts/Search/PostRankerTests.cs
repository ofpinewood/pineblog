using FluentAssertions;
using Opw.PineBlog.Entities;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Opw.PineBlog.Posts.Search
{
    public class PostRankerTests
    {
        [Fact]
        public void Rank_Should_0Posts_WhenPostsNull()
        {
            var query = "c# dotnet pineblog";

            var results = new PostRanker().Rank(null, query);

            results.Should().HaveCount(0);
        }

        [Fact]
        public void Rank_Should_2Posts_WhenQueryNull()
        {
            var posts = new List<Post>
            {
                new Post { Slug = "1", Title = "pineblog", Categories = "pineblog", Description = "pineblog", Content = "pinelog" },
                new Post { Slug = "2", Title = "pineblog", Categories = "pineblog", Description = "pineblog", Content = "pinelog" },
            };

            var results = new PostRanker().Rank(posts, null);

            results.Should().HaveCount(2);
        }

        [Fact]
        public void Rank_Should_2Posts_WhenPostProperiesNull()
        {
            var query = "c# dotnet pineblog";
            var posts = new List<Post>
            {
                new Post { Slug = "1", Title = "pineblog", Categories = "pineblog", Description = "pineblog", Content = "pinelog" },
                new Post { Slug = "2", Title = null, Categories = null, Description = null, Content = null },
            };

            var results = new PostRanker().Rank(posts, query);

            results.Should().HaveCount(2);
        }

        [Fact]
        public void Rank_Should_PostsRankedCorrectly_ForOneMatchingTerm()
        {
            var query = "c# dotnet pineblog";
            var posts = new List<Post>
            {
                new Post { Slug = "5", Title = "xxx", Categories = "xxx", Description = "xxx", Content = "xxx" },
                new Post { Slug = "4", Title = "pineblog", Categories = "xxx", Description = "xxx", Content = "xxx" },
                new Post { Slug = "3", Title = "pineblog", Categories = "pineblog", Description = "xxx", Content = "xxx" },
                new Post { Slug = "2", Title = "pineblog", Categories = "pineblog", Description = "pineblog", Content = "xxx" },
                new Post { Slug = "1", Title = "pineblog", Categories = "pineblog", Description = "pineblog", Content = "pinelog" },
            };

            var results = new PostRanker().Rank(posts, query);

            results.Should().HaveCount(5);
            results.Select(p => p.Slug).Should().BeEquivalentTo(new string[] { "1", "2", "3", "4", "5" });
        }
    }
}
