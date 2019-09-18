
namespace Opw.PineBlog
{
    /// <summary>
    /// Represents the possible PineBlog options for services.
    /// </summary>
    public class PineBlogOptions
    {
        /// <summary>
        /// Blog Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Blog description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Number of items per page.
        /// </summary>
        public int ItemsPerPage { get; set; }

        /// <summary>
        /// Cover URL.
        /// </summary>
        public string CoverUrl { get; set; }

        /// <summary>
        /// Cover caption text.
        /// </summary>
        public string CoverCaption { get; set; }

        /// <summary>
        /// Cover link.
        /// </summary>
        public string CoverLink { get; set; }

        public string PagingUrlPartFormat { get; set; }
        public string CategoryUrlPartFormat { get; set; }
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
        /// The URL of the location where the images and other files are stored.
        /// Can be the web host, a CDN or a local host.
        /// </summary>
        public string FileBaseUrl { get; set; }

        /// <summary>
        /// Database connection string name.
        /// </summary>
        public string ConnectionStringName { get; set; }

        /// <summary>
        /// Azure storage connection string.
        /// </summary>
        public string AzureStorageConnectionString { get; set; }

        /// <summary>
        /// Azure storage blob container name.
        /// </summary>
        public string AzureStorageBlobContainerName { get; set; }
    }
}
