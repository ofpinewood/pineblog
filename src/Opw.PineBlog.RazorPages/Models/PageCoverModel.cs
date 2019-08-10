
namespace Opw.PineBlog.Models
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
        public PostListType? PostListType { get; set; }

        /// <summary>
        /// Category filter.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Implementation of PageCoverModel.
        /// </summary>
        /// <param name="title">The page title.</param>
        /// <param name="coverUrl">Cover URL.</param>
        /// <param name="coverCaption">Cover caption.</param>
        /// <param name="coverLink">Cover link.</param>
        /// <param name="postListType">Post list type.</param>
        /// <param name="category">Category filter.</param>
        public PageCoverModel(string title, string coverUrl, string coverCaption, string coverLink, PostListType? postListType, string category)
        {
            Title = title;
            CoverUrl = coverUrl;
            CoverCaption = coverCaption;
            CoverLink = coverLink;
            PostListType = postListType;
            Category = category;
        }
    }
}
