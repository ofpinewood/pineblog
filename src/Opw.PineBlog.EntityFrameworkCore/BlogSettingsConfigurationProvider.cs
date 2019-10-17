using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Opw.EntityFrameworkCore;
using Opw.PineBlog.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Opw.PineBlog.EntityFrameworkCore
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
                EntityChangeObserver.Instance.Changed += EntityChangeObserver_Changed;
        }

        private void EntityChangeObserver_Changed(object sender, EntityChangeEventArgs e)
        {
            if (e.Entry.Entity.GetType() != typeof(BlogSettings))
                return;

            Thread.Sleep(_source.ReloadDelay);
            Load();
        }

        /// <summary>
        /// Load the blog settings configuration from the database.
        /// </summary>
        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<BlogEntityDbContext>();
            _source.OptionsAction(builder);

            using (var context = new BlogEntityDbContext(builder.Options))
            {
                if (!context.Database.CanConnect())
                    return;

                var settings = context.BlogSettings.SingleOrDefault();
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
}
