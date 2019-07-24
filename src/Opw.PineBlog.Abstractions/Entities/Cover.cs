using Opw.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Opw.PineBlog.Entities
{
    public class Cover : IEntity<Guid>, IEntityCreated, IEntityModified
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        /// <summary>
        /// URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Caption text.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// link.
        /// </summary>
        public string Link { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
