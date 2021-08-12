using Microsoft.Extensions.Configuration;

namespace Opw.PineBlog.GitDb
{
    /// <summary>
    /// Blog settings configuration class for Git based <see cref="IConfigurationSource"/>.
    /// </summary>
    public class BlogSettingsConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// Determines whether the source will be loaded if the underlying entity changes.
        /// </summary>
        public bool ReloadOnChange { get; set; }

        /// <summary>
        /// Number of milliseconds that reload will wait before calling Load. This helps
        /// avoid triggering reload before a changes is completely saved. Default is 500.
        /// </summary>
        public int ReloadDelay { get; set; } = 500;

        /// <summary>
        /// Builds the <see cref="IConfigurationProvider" /> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder" />.</param>
        /// <returns>An <see cref="IConfigurationProvider" /></returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new BlogSettingsConfigurationProvider(this);
        }
    }
}
