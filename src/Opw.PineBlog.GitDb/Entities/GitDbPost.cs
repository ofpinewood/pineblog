using Opw.PineBlog.Entities;
using System;

namespace Opw.PineBlog.GitDb.Entities
{
    public class GitDbPost
    {
        public string AuthorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Categories { get; set; }
        public DateTime? Published { get; set; }
        public string Slug { get; set; }
        public string CoverUrl { get; set; }
        public string CoverCaption { get; set; }
        public string CoverLink { get; set; }
    }
}
