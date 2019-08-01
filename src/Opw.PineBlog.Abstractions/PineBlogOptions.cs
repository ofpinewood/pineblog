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
        /// The URL of the CDN where the images and other files are accessible.
        /// Can also be the web host or a local hostname if no CDN is used.
        /// </summary>
        public string CdnUrl { get; set; }

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
