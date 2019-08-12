using Opw.PineBlog.Models;

namespace Opw.PineBlog.RazorPages.Models
{
    /// <summary>
    /// Model for the _PageCover partial view.
    /// </summary>
    public class PageCoverModel
    {
        /// <summary>
        /// The page title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Cover URL.
        /// </summary>
        public string CoverUrl { get; set; }

        /// <summary>
        /// Cover caption text.
        /// </summary>
        public string CoverCaption { get; set; }

        /// <summary>
        /// Cover link.
        /// </summary>
        public string CoverLink { get; set; }

        /// <summary>
        /// Post list type.
        /// </summary>
        public PostListType PostListType { get; set; }

        /// <summary>
        /// Category filter.
        /// </summary>
        public string Category { get; set; }
    }
}
