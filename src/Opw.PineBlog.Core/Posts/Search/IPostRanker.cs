using Opw.PineBlog.Entities;
using System.Collections.Generic;

namespace Opw.PineBlog.Posts.Search
{
    public interface IPostRanker
    {
        IEnumerable<Post> Rank(IEnumerable<Post> posts, string query);
    }
}