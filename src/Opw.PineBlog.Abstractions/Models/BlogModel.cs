
using Opw.PineBlog.Entities;

namespace Opw.PineBlog.Models
{
    public class BlogModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Cover Cover { get; set; }

        public BlogModel(PineBlogOptions options)
        {
            Title = options.Title;
            Description = options.Description;
            Cover = options.Cover;
        }
    }
}
