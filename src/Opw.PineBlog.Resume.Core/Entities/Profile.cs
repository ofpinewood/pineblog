using System.Collections.Generic;

namespace Opw.PineBlog.Resume.Entities
{
    public class Profile : Entity
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

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
    }
}
