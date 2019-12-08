
namespace Opw.PineBlog.Resume.Entities
{
    public class Education : Entity
    {
        public string School { get; set; }
        public string Degree { get; set; }
        public string FieldOfStudy { get; set; }
        public string Description { get; set; }

        public int StartYear { get; set; }
        public int? EndYear { get; set; }
    }
}
