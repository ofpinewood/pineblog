using Opw.PineBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Opw.PineBlog.Posts.Search
{
    // TODO: add more test for ranking (hits count)
    public class PostRanker : IPostRanker
    {
        public IEnumerable<Post> Rank(IEnumerable<Post> posts, string query)
        {
            if (posts == null || !posts.Any())
            {
                return new List<Post>();
            }

            var terms = query.ParseTerms();
            var rankedPosts = new List<Tuple<Post, int>>();

            foreach (var post in posts)
            {
                var rank = 0;
                foreach (var term in terms)
                {
                    int hits;
                    if (!string.IsNullOrWhiteSpace(post.Title) && post.Title.ToLower().Contains(term))
                    {
                        hits = Regex.Matches(post.Title.ToLower(), term).Count;
                        rank += hits * 10;
                    }
                    if (!string.IsNullOrWhiteSpace(post.Categories) && post.Categories.ToLower().Contains(term))
                    {
                        hits = Regex.Matches(post.Categories.ToLower(), term).Count;
                        rank += hits * 10;
                    }
                    if (!string.IsNullOrWhiteSpace(post.Description) && post.Description.ToLower().Contains(term))
                    {
                        hits = Regex.Matches(post.Description.ToLower(), term).Count;
                        rank += hits * 3;
                    }
                    if (!string.IsNullOrWhiteSpace(post.Content) && post.Content.ToLower().Contains(term))
                    {
                        hits = Regex.Matches(post.Content.ToLower(), term).Count;
                        rank += hits * 1;
                    }
                }

                rankedPosts.Add(new Tuple<Post, int>(post, rank));
            }

            return rankedPosts.OrderByDescending(t => t.Item2).Select(t => t.Item1).ToList();
        }
    }
}
