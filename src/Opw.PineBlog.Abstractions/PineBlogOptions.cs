using Opw.PineBlog.Entities;

namespace Opw
{
    /// <summary>
    /// Represents the possible PineBlog options for services.
    /// </summary>
    public class PineBlogOptions
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int ItemsPerPage { get; set; }
        public string PagingUrlPartFormat { get; set; }
        public Cover Cover { get; set; }
        public bool CreateAndSeedDatabases { get; set; }

        /// <summary>
        /// Connection string.
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
