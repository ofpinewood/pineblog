using Microsoft.Extensions.Options;

namespace Opw.PineBlog.Files
{
    /// <summary>
    /// Helper for working with file URLs.
    /// </summary>
    public class FileUrlHelper
    {
        private readonly IOptions<PineBlogOptions> _blogOptions;

        /// <summary>
        /// Implementation of FileUrlHelper.
        /// </summary>
        /// <param name="blogOptions">The blog options.</param>
        public FileUrlHelper(IOptions<PineBlogOptions> blogOptions)
        {
            _blogOptions = blogOptions;
        }

        /// <summary>
        /// Returns a new string in which all occurrences of the BaseUrl replaced with the UrlFormat.
        /// </summary>
        public string ReplaceBaseUrlWithUrlFormat(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return s;

            return s.Replace(GetBaseUrl(), "%URL%");
        }

        /// <summary>
        /// Returns a new string in which all occurrences of the UrlFormat replaced with the BaseUrl.
        /// </summary>
        public string ReplaceUrlFormatWithBaseUrl(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return s;

            return s.Replace("%URL%", GetBaseUrl());
        }

        /// <summary>
        /// Get the base URL for where the files are located.
        /// </summary>
        public string GetBaseUrl()
        {
            var url = _blogOptions.Value.FileBaseUrl;
            if (!string.IsNullOrWhiteSpace(_blogOptions.Value.AzureStorageBlobContainerName))
                url += "/" + _blogOptions.Value.AzureStorageBlobContainerName;

            return url;
        }
    }
}
