using Microsoft.Extensions.Options;

namespace Opw.PineBlog.Files
{
    /// <summary>
    /// Helper for working with file paths.
    /// </summary>
    public class FilePathHelper
    {
        private readonly IOptions<PineBlogOptions> _blogOptions;

        /// <summary>
        /// Implementation of FilePathHelper.
        /// </summary>
        /// <param name="blogOptions">The blog options.</param>
        public FilePathHelper(IOptions<PineBlogOptions> blogOptions)
        {
            _blogOptions = blogOptions;
        }

        /// <summary>
        /// Get the path format form the full URL.
        /// </summary>
        /// <param name="url">URL of the file.</param>
        /// <returns>Format; "%URL%filename.ext" or "%URL%path/filename.ext".</returns>
        public string GetPathFormat(string url)
        {
            string path;
            if (!string.IsNullOrWhiteSpace(_blogOptions.Value.AzureStorageBlobContainerName))
                path = url.Substring(url.IndexOf(_blogOptions.Value.AzureStorageBlobContainerName) + _blogOptions.Value.AzureStorageBlobContainerName.Length);
            else
                path = url.Substring(url.IndexOf(GetBaseUrl()) + GetBaseUrl().Length);
            return $"%URL%{path}";
        }

        /// <summary>
        /// Get the base URL for where the files are located.
        /// </summary>
        public string GetBaseUrl()
        {
            var url = _blogOptions.Value.CdnUrl;
            if (!string.IsNullOrWhiteSpace(_blogOptions.Value.AzureStorageBlobContainerName))
                url += "/" + _blogOptions.Value.AzureStorageBlobContainerName;

            return url;
        }
    }
}
