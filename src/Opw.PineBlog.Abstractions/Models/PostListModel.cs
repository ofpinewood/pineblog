using Opw.PineBlog.Entities;
using System.Collections.Generic;

namespace Opw.PineBlog.Models
{
    public class PostListModel
    {
        public BlogModel Blog { get; set; }
        //public Author Author { get; set; } // posts by author
        //public string Category { get; set; } // posts by category

        public IEnumerable<Post> Posts { get; set; }
        public Pager Pager { get; set; }

        public PostListType PostListType { get; set; }
    }
}
