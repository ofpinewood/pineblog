using Microsoft.Extensions.Configuration;

namespace Opw.PineBlog.MongoDb
{
    /// <summary>
    /// Provides extension methods for the application configuration builder interface.
    /// </summary>
    // TODO: implement when BlogSettingsConfigurationProvider for MongoDb has been implemented
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds PineBlog MongoDb application configuration.
        /// </summary>
        /// <param name="builder">The configuration builder.</param>
        /// <param name="reloadOnChange">Determines whether the source will be loaded if the underlying entity changes.</param>
        /// <returns>The original configuration object.</returns>
        public static IConfigurationBuilder AddPineBlogMongoDbConfiguration(this IConfigurationBuilder builder, bool reloadOnChange = false)
        {
            return builder;
        }
    }
}
