using Microsoft.Extensions.Configuration;
using Opw.PineBlog.Entities;
using System.Collections.Generic;
using System.Threading;

// TODO: implement for GitDb
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
            //if (e.Document.CollectionNamespace.CollectionName != CollectionHelper.GetName<BlogSettings>())
            //    return;

            Thread.Sleep(_source.ReloadDelay);
            Load();
        }

        /// <summary>
        /// Load the blog settings configuration from the database.
        /// </summary>
        public override void Load()
        {
            //var client = new MongoClient(_source.ConnectionString);
            //var database = client.GetDatabase(_source.DatabaseName);
            //var collection = database.GetCollection<BlogSettings>(CollectionHelper.GetName<BlogSettings>());

            //var settings = collection.Find(Builders<BlogSettings>.Filter.Empty).SingleOrDefault();
            //if (settings == null) return;

            //Data = new Dictionary<string, string>();
            //Data.Add($"{nameof(PineBlogOptions)}:{nameof(PineBlogOptions.Title)}", settings.Title);
            //Data.Add($"{nameof(PineBlogOptions)}:{nameof(PineBlogOptions.Description)}", settings.Description);
            //Data.Add($"{nameof(PineBlogOptions)}:{nameof(PineBlogOptions.CoverCaption)}", settings.CoverCaption);
            //Data.Add($"{nameof(PineBlogOptions)}:{nameof(PineBlogOptions.CoverLink)}", settings.CoverLink);
            //Data.Add($"{nameof(PineBlogOptions)}:{nameof(PineBlogOptions.CoverUrl)}", settings.CoverUrl);
        }
    }
}
