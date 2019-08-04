
using Opw.PineBlog.Entities;

namespace Opw.PineBlog.Models
{
    public class BlogModel
    {
        public string Title { get; set; }
        public string Description { get; set; }

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

        public BlogModel(PineBlogOptions options)
        {
            Title = options.Title;
            Description = options.Description;
            CoverUrl = options.CoverUrl;
            CoverCaption = options.CoverCaption;
            CoverLink = options.CoverLink;
        }
    }
}
