using Opw.EntityFrameworkCore;
using System;

namespace Opw.PineBlog.Entities
{
    public class Post : IEntity<Guid>, IEntityCreated, IEntityModified
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public Guid AuthorId { get; set; }
        public virtual Author Author { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Categories { get; set; }
        public virtual Cover Cover { get; set; }

        //public int PostViews { get; set; }
        //public double Rating { get; set; }

        //public bool IsFeatured { get; set; }

        public DateTime? Published { get; set; }
    }
}
