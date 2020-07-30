using Opw.Entities;
using System;
using System.Collections.Generic;

namespace Opw.PineBlog.Entities
{
    public class Author : IEntity<Guid>, IEntityCreated, IEntityModified
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public string Avatar { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
