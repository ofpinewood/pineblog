using Opw.EntityFrameworkCore;
using System;

namespace Opw.PineBlog.Entities
{
    public class Cover : IEntity<Guid>, IEntityCreated, IEntityModified
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string Url { get; set; }
        public string Caption { get; set; }
        public string Link { get; set; }
    }
}
