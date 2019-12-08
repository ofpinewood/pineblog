using Opw.EntityFrameworkCore;
using System;

namespace Opw.PineBlog.Resume.Entities
{
    public abstract class Entity : IEntity<Guid>, IEntityCreated, IEntityModified
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
