using Microsoft.Extensions.Configuration;

namespace Opw.PineBlog.GitDb
{
    /// <summary>
    /// Provides extension methods for the application configuration builder interface.
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds PineBlog Git application configuration.
        /// </summary>
        /// <param name="builder">The configuration builder.</param>
        /// <param name="reloadOnChange">Determines whether the source will be loaded if the underlying entity changes.</param>
        /// <returns>The original configuration object.</returns>
        public static IConfigurationBuilder AddPineBlogGitConfiguration(this IConfigurationBuilder builder, bool reloadOnChange = false)
        {
            //var configuration = builder.Build();
            //var connectionStringName = configuration.GetSection(nameof(PineBlogOptions)).GetValue<string>(nameof(PineBlogOptions.ConnectionStringName));
            //var connectionString = configuration.GetConnectionString(connectionStringName);

            //var optionsAction = DbContextOptionsHelper.Configure(connectionString);
            //return builder.Add(new BlogSettingsConfigurationSource
            //{
            //    OptionsAction = optionsAction,
            //    ReloadOnChange = reloadOnChange
            //});
        }
    }
}
