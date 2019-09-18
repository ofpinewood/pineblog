using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Opw.PineBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Opw.PineBlog.EntityFrameworkCore
{
    /// <summary>
    /// Provides blog settings configuration key/values for the application.
    /// </summary>
    public class BlogSettingsConfigurationProvider : ConfigurationProvider, IBlogSettingsConfigurationProvider
    {
        Action<DbContextOptionsBuilder> OptionsAction { get; }

        /// <summary>
        /// Implementation of BlogSettingsConfigurationProvider.
        /// </summary>
        public BlogSettingsConfigurationProvider(Action<DbContextOptionsBuilder> optionsAction)
        {
            OptionsAction = optionsAction;
            // TODO: for ReloadOnChange check https://github.com/aspnet/Extensions/blob/master/src/Configuration/Config.FileExtensions/src/FileConfigurationProvider.cs
        }

        /// <summary>
        /// Trigger a reload.
        /// </summary>
        public void Reload()
        {
            Load();
            OnReload();
        }

        /// <summary>
        /// Load the blog settings configuration from the database.
        /// </summary>
        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<BlogEntityDbContext>();

            OptionsAction(builder);

            using (var context = new BlogEntityDbContext(builder.Options))
            {
                context.Database.EnsureCreated();

                var settings = context.BlogSettings.SingleOrDefault();
                if (settings == null) return;
                //TODO: save the defaults?

                Data = new Dictionary<string, string>();
                Data.Add($"{nameof(BlogSettings)}.{nameof(BlogSettings.Title)}", settings.Title);
                Data.Add($"{nameof(BlogSettings)}.{nameof(BlogSettings.Description)}", settings.Description);
                Data.Add($"{nameof(BlogSettings)}.{nameof(BlogSettings.CoverCaption)}", settings.CoverCaption);
                Data.Add($"{nameof(BlogSettings)}.{nameof(BlogSettings.CoverLink)}", settings.CoverLink);
                Data.Add($"{nameof(BlogSettings)}.{nameof(BlogSettings.CoverUrl)}", settings.CoverUrl);
            }
        }
    }
}
