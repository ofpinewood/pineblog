using Opw.PineBlog.Entities;

namespace Opw.PineBlog.Models
{
    public class PostModel
    {
        public BlogModel Blog { get; set; }
        public Post Post { get; set; }
        public Post Previous { get; set; }
        public Post Next { get; set; }
    }
}
