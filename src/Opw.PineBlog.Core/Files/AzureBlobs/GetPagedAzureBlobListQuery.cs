using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using Opw.PineBlog.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Files.AzureBlobs
{
    /// <summary>
    /// Query that gets a FileListModel using Azure blob storage.
    /// </summary>
    public class GetPagedAzureBlobListQuery : IGetPagedFileListQuery
    {
        /// <summary>
        /// The requested page.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// The number of items per page, if not set the BlogOptions.ItemsPerPage will be used.
        /// </summary>
        public int ItemsPerPage { get; set; }

        /// <summary>
        /// The directory path to get the files from.
        /// </summary>
        public string DirectoryPath { get; set; }

        /// <summary>
        /// The file type to filter on.
        /// </summary>
        public FileType FileType { get; set; }

        /// <summary>
        /// Handler for the GetPagedAzureBlobListQuery.
        /// </summary>
        public class Handler : IGetPagedFileListQueryHandler<GetPagedAzureBlobListQuery>
        {
            private readonly AzureBlobHelper _azureBlobHelper;
            private readonly IOptionsSnapshot<PineBlogOptions> _blogOptions;

            /// <summary>
            /// Implementation of GetPagedAzureBlobListQuery.Handler.
            /// </summary>
            /// <param name="azureBlobHelper">Azure blob helper.</param>
            /// <param name="blogOptions">Blog options.</param>
            public Handler(AzureBlobHelper azureBlobHelper, IOptionsSnapshot<PineBlogOptions> blogOptions)
            {
                _azureBlobHelper = azureBlobHelper;
                _blogOptions = blogOptions;
            }

            /// <summary>
            /// Handle the GetPagedAzureBlobListQuery request.
            /// </summary>
            /// <param name="request">The GetPagedAzureBlobListQuery request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<FileListModel>> Handle(GetPagedAzureBlobListQuery request, CancellationToken cancellationToken)
            {
                var blobContainerClient = await _azureBlobHelper.GetBlobContainerClientAsync(cancellationToken);
                if (!blobContainerClient.IsSuccess)
                    return Result<FileListModel>.Fail(blobContainerClient.Exception);

                var pager = new Pager(request.Page, request.ItemsPerPage);
                var files = await GetPagedListAsync(pager, blobContainerClient.Value, request.DirectoryPath, request.FileType, cancellationToken);

                var model = new FileListModel
                {
                    Files = files,
                    Pager = pager
                };

                return Result<FileListModel>.Success(model);
            }

            private async Task<IEnumerable<FileModel>> GetPagedListAsync(
                Pager pager,
                BlobContainerClient blobContainerClient,
                string directoryPath,
                FileType fileType,
                CancellationToken cancellationToken)
            {
                AsyncPageable<BlobItem> blobs = blobContainerClient.GetBlobsAsync(BlobTraits.None, BlobStates.None, directoryPath, cancellationToken);

                var files = new List<string>();
                await foreach (BlobItem blob in blobs)
                {
                    files.Add(blob.Name);
                }

                var skip = (pager.CurrentPage - 1) * pager.ItemsPerPage;

                if (fileType != FileType.All)
                    files = files.Where(f => fileType.IsFileTypeSupported(f.GetMimeType())).ToList();

                var count = files.Count();

                pager.Configure(count, _blogOptions.Value.PagingUrlPartFormat);

                return files
                    .OrderBy(f => f)
                    .Skip(skip)
                    .Take(pager.ItemsPerPage)
                    .Select(f => new FileModel { Url = GetUrl(blobContainerClient, f), FileName = Path.GetFileName(f), MimeType = f.GetMimeType() });
            }

            private string GetUrl(BlobContainerClient blobContainerClient, string blobName)
            {
                return blobContainerClient.Uri + "/" + blobName;
            }
        }
    }
}
