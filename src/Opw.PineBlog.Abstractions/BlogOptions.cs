using Opw.PineBlog.Entities;

namespace Opw
{
    public class BlogOptions
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PostUrlFormat { get; set; }
        public int ItemsPerPage { get; set; }
        public Cover Cover { get; set; }
        public bool CreateAndSeedDatabases { get; set; }
    }
}
