using Microsoft.Extensions.Configuration;

namespace Opw.PineBlog.MongoDb
{
    /// <summary>
    /// Provides blog settings configuration key/values for the application.
    /// </summary>
    // TODO: implement BlogSettingsConfigurationProvider for MongoDb
    public class BlogSettingsConfigurationProvider : ConfigurationProvider, IBlogSettingsConfigurationProvider
    {
        /// <summary>
        /// Load the blog settings configuration from the database.
        /// </summary>
        public override void Load()
        {
        }
    }
}
