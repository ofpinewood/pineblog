
namespace Opw.PineBlog.Resume
{
    /// <summary>
    /// Represents the possible PineBlog:Resume options for services.
    /// </summary>
    public class PineBlogResumeOptions
    {
        /// <summary>
        /// Create and seed databases.
        /// </summary>
        public bool CreateAndSeedDatabases { get; set; }

        /// <summary>
        /// The version of the current running code.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The theme for the blog, defaults to "default".
        /// </summary>
        public string Theme { get; set; } = "default";

        /// <summary>
        /// Database connection string name.
        /// </summary>
        public string ConnectionStringName { get; set; }
    }
}
