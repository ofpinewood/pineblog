using Opw.EntityFrameworkCore;
using System;

namespace Opw.PineBlog.Entities
{
    public class Settings : IEntityCreated, IEntityModified
    {
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual Cover Cover { get; set; }
    }
}
