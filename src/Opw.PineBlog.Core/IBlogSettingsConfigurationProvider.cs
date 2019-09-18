namespace Opw.PineBlog
{
    /// <summary>
    /// Provides blog settings configuration key/values for the application.
    /// </summary>
    public interface IBlogSettingsConfigurationProvider
    {
        /// <summary>
        /// Load the blog settings configuration from the database.
        /// </summary>
        void Load();
    }
}