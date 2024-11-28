using Microsoft.Extensions.Options;
using Opw.PineBlog.Files.AzureBlobs;

namespace Opw.PineBlog.Files
{
    /// <summary>
    /// Creates the correct IGetPagedFileListQuery based on the configuration.
    /// </summary>
    public class GetPagedFileListQueryFactory : IGetPagedFileListQueryFactory
    {
        private readonly IOptionsSnapshot<PineBlogOptions> _blogOptions;

        /// <summary>
        /// Implementation of GetPagedFileListQueryFactory.
        /// </summary>
        /// <param name="blogOptions">Blog options.</param>
        public GetPagedFileListQueryFactory(IOptionsSnapshot<PineBlogOptions> blogOptions)
        {
            _blogOptions = blogOptions;
        }

        /// <summary>
        /// Create a IGetPagedFileListQuery based on the configuration.
        /// </summary>
        /// <param name="page">The requested page.</param>
        /// <param name="itemsPerPage">The number of items per page, if not set the BlogOptions.ItemsPerPage will be used.</param>
        /// <param name="directoryPath">The directory path to get the files from.</param>
        /// <param name="fileType">The file type to filter on.</param>
        /// <returns></returns>
        public IGetPagedFileListQuery Create(int? page, int? itemsPerPage, string directoryPath, FileType? fileType)
        {
            if (!page.HasValue) page = 1;
            if (!itemsPerPage.HasValue) itemsPerPage = _blogOptions.Value.ItemsPerPage;
            if (string.IsNullOrWhiteSpace(directoryPath)) directoryPath = "";
            if (!fileType.HasValue) fileType = FileType.All;

            return new GetPagedAzureBlobListQuery
            {
                Page = page.Value,
                ItemsPerPage = itemsPerPage.Value,
                DirectoryPath = directoryPath,
                FileType = fileType.Value
            };
        }
    }
}
