using Opw.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Opw.PineBlog.Resume.Entities
{
    /// <summary>
    /// User's profile (resume).
    /// </summary>
    public class Profile : IEntityCreated, IEntityModified
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string UserName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
                
        public string Slug { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string Headline { get; set; }
        public string Industry { get; set; }
        public string Summary { get; set; }

        public string Country { get; set; }
        public string Region { get; set; }

        public virtual ICollection<Link> Links { get; set; }
        public virtual ICollection<Experience> Experiences { get; set; }
        public virtual ICollection<Education> Education { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
        public virtual ICollection<Language> Languages { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
