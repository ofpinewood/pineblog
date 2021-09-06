using Microsoft.Extensions.Configuration;
using Opw.PineBlog.Entities;
using Opw.PineBlog.GitDb.LibGit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Opw.PineBlog.GitDb
{
    /// <summary>
    /// Provides blog settings configuration key/values for the application.
    /// </summary>
    public class BlogSettingsConfigurationProvider : ConfigurationProvider, IBlogSettingsConfigurationProvider
    {
        private readonly BlogSettingsConfigurationSource _source;

        /// <summary>
        /// Implementation of BlogSettingsConfigurationProvider.
        /// </summary>
        public BlogSettingsConfigurationProvider(BlogSettingsConfigurationSource source)
        {
            _source = source;

            if (_source.ReloadOnChange)
                FileChangeObserver.Instance.Changed += FileChangeObserver_Changed;
        }

        private void FileChangeObserver_Changed(object sender, FileChangeEventArgs e)
        {
            if (!e.File.Equals(GitDbConstants.BlogSettingsFile, StringComparison.OrdinalIgnoreCase))
                return;

            Thread.Sleep(_source.ReloadDelay);
            Load();
        }

        /// <summary>
        /// Load the blog settings configuration from the repository.
        /// </summary>
        public override void Load()
        {
            var gitDbContext = GitDbContext.Create(_source.Options);

            IDictionary<string, byte[]> files;

            try
            {
                files = gitDbContext.GetFiles(new string[] { PathHelper.Build(_source.Options.RootPath, GitDbConstants.BlogSettingsFile) });
            }
            catch
            {
                return;
            }

            var json = Encoding.UTF8.GetString(files.Values.Single());
            var settings = JsonSerializer.Deserialize<BlogSettings>(json, new JsonSerializerOptions { AllowTrailingCommas = true });
            if (settings == null) return;

            Data = new Dictionary<string, string>();
            Data.Add($"{nameof(PineBlogOptions)}:{nameof(PineBlogOptions.Title)}", settings.Title);
            Data.Add($"{nameof(PineBlogOptions)}:{nameof(PineBlogOptions.Description)}", settings.Description);
            Data.Add($"{nameof(PineBlogOptions)}:{nameof(PineBlogOptions.CoverCaption)}", settings.CoverCaption);
            Data.Add($"{nameof(PineBlogOptions)}:{nameof(PineBlogOptions.CoverLink)}", settings.CoverLink);
            Data.Add($"{nameof(PineBlogOptions)}:{nameof(PineBlogOptions.CoverUrl)}", settings.CoverUrl);
        }
    }
}
