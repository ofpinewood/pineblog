
namespace Opw.PineBlog.Models
{
    /// <summary>
    /// Feed model.
    /// </summary>
    public class FeedModel<T> where T : class
    {
        /// <summary>
        /// The feed.
        /// </summary>
        public T Feed { get; set; }

        /// <summary>
        /// The content type for the feed.
        /// </summary>
        public string ContentType { get; set; }
    }
}
