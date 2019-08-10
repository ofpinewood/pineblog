using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;

namespace Opw.PineBlog.RazorPages
{
    /// <summary>
    /// Configures the StaticFileOptions type.
    /// </summary>
    public class StaticFilePostConfigureOptions : IPostConfigureOptions<StaticFileOptions>
    {
        private readonly IHostingEnvironment _environment;

        /// <summary>
        /// Initializes the StaticFilePostConfigureOptions.
        /// </summary>
        public StaticFilePostConfigureOptions(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        /// <summary>
        /// Invoked to configure a TOptions instance.
        /// </summary>
        /// <param name="name">The name of the options instance being configured.</param>
        /// <param name="options">The options instance to configured.</param>
        public void PostConfigure(string name, StaticFileOptions options)
        {
            options = options ?? throw new ArgumentNullException(nameof(options));

            // Basic initialization in case the options weren't initialized by any other component
            options.ContentTypeProvider = options.ContentTypeProvider ?? new FileExtensionContentTypeProvider();
            if (options.FileProvider == null && _environment.WebRootFileProvider == null)
                throw new InvalidOperationException("Missing FileProvider.");

            options.FileProvider = options.FileProvider ?? _environment.WebRootFileProvider;

            var basePath = "wwwroot";
            var filesProvider = new ManifestEmbeddedFileProvider(GetType().Assembly, basePath);
            options.FileProvider = new CompositeFileProvider(options.FileProvider, filesProvider);
        }
    }
}
