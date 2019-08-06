using Opw.PineBlog.Entities;
using Opw.PineBlog.Files;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Helper for working with file URLs in posts.
    /// </summary>
    public class PostUrlHelper
    {
        private readonly FileUrlHelper _fileUrlHelper;

        /// <summary>
        /// Implementation of PostUrlHelper.
        /// </summary>
        /// <param name="fileUrlHelper">File URL helper.</param>
        public PostUrlHelper(FileUrlHelper fileUrlHelper)
        {
            _fileUrlHelper = fileUrlHelper;
        }

        /// <summary>
        /// Returns a new string in which all occurrences of the BaseUrl replaced with the UrlFormat.
        /// </summary>
        public Post ReplaceBaseUrlWithUrlFormat(Post post)
        {
            if (post == null) return post;

            post.Content = _fileUrlHelper.ReplaceBaseUrlWithUrlFormat(post.Content);
            post.CoverUrl = _fileUrlHelper.ReplaceBaseUrlWithUrlFormat(post.CoverUrl);
            return post;
        }

        /// <summary>
        /// Returns a new string in which all occurrences of the UrlFormat replaced with the BaseUrl.
        /// </summary>
        public Post ReplaceUrlFormatWithBaseUrl(Post post)
        {
            if (post == null) return post;

            post.Content = _fileUrlHelper.ReplaceUrlFormatWithBaseUrl(post.Content);
            post.CoverUrl = _fileUrlHelper.ReplaceUrlFormatWithBaseUrl(post.CoverUrl);
            return post;
        }
    }
}
