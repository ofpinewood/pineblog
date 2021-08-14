using Opw.PineBlog.Entities;

namespace Opw.PineBlog.GitDb.Entities
{
    public class GitDbPost : Post
    {
        public new string AuthorId { get; set; }
    }
}
