using Opw.PineBlog.Entities;
using System.Collections.Generic;

namespace Opw.PineBlog.Models
{
    /// <summary>
    /// PostListModel.
    /// </summary>
    public class PostListModel
    {
        /// <summary>
        /// Blog model.
        /// </summary>
        public BlogModel Blog { get; set; }

        /// <summary>
        /// The category that was filtered on, or NULL.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The list of posts for the current page.
        /// </summary>
        public IEnumerable<Post> Posts { get; set; }

        /// <summary>
        /// Pager.
        /// </summary>
        public Pager Pager { get; set; }

        /// <summary>
        /// The type of the post list.
        /// </summary>
        public PostListType PostListType { get; set; }
    }
}
