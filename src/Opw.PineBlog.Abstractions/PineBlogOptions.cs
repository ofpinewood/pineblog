
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
        public bool CreateAndSeedDatabases { get; set; }

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

        /// <summary>
        /// The URL of the location where the images and other files are stored.
        /// Can be the web host, a CDN or a local host.
        /// </summary>
        public string FileBaseUrl { get; set; }

        /// <summary>
        /// The path for the folder where the cover images are stored.
        /// </summary>
        public string CoverImagesPath { get; set; }

        /// <summary>
        /// Database connection string.
        /// </summary>
        // TODO: use this connection string
        public string ConnectionString { get; set; }

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
