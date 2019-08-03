using MediatR;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Options;
using Opw.PineBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Files.Azure
{
    /// <summary>
    /// Query that gets a FileListModel using Azure blob storage.
    /// </summary>
    public class GetPagedAzureBlobListQuery : IRequest<Result<FileListModel>>
    {
        /// <summary>
        /// The pager.
        /// </summary>
        public Pager Pager { get; set; }

        /// <summary>
        /// The directory path.
        /// </summary>
        public string DirectoryPath { get; set; }

        /// <summary>
        /// Handler for the GetPagedAzureBlobListQuery.
        /// </summary>
        public class Handler : IRequestHandler<GetPagedAzureBlobListQuery, Result<FileListModel>>
        {
            private readonly AzureBlobHelper _azureBlobHelper;
            private readonly IOptions<PineBlogOptions> _blogOptions;

            /// <summary>
            /// Implementation of GetPagedAzureBlobListQuery.Handler.
            /// </summary>
            /// <param name="azureBlobHelper">Azure blob helper.</param>
            /// <param name="blogOptions">Blog options.</param>
            public Handler(AzureBlobHelper azureBlobHelper, IOptions<PineBlogOptions> blogOptions)
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
                var cloudBlobContainer = await _azureBlobHelper.GetCloudBlobContainerAsync(cancellationToken);
                if (!cloudBlobContainer.IsSuccess)
                    return Result<FileListModel>.Fail(cloudBlobContainer.Exception);

                var pager = request.Pager;
                var files = await GetPagedListAsync(pager, cloudBlobContainer.Value, request.DirectoryPath, cancellationToken);

                var model = new FileListModel
                {
                    Files = files,
                    Pager = pager
                };

                return Result<FileListModel>.Success(model);
            }

            private async Task<IEnumerable<string>> GetPagedListAsync(
                Pager pager,
                CloudBlobContainer cloudBlobContainer,
                string directoryPath,
                CancellationToken cancellationToken)
            {
                var directory = cloudBlobContainer.GetDirectoryReference(directoryPath);
                var blobs = await ListAsync(directory, cancellationToken);
                var files = blobs.Select(b => b.Uri.AbsoluteUri);

                var skip = (pager.CurrentPage - 1) * pager.ItemsPerPage;

                var count = files.Count();

                pager.Configure(count, _blogOptions.Value.PagingUrlPartFormat);

                return files
                    .OrderBy(f => f)
                    .Skip(skip)
                    .Take(pager.ItemsPerPage);
            }

            private async Task<List<IListBlobItem>> ListAsync(CloudBlobDirectory directory, CancellationToken cancellationToken = default(CancellationToken))
            {
                var blobsInDirectory = new List<IListBlobItem>();
                BlobContinuationToken blobContinuationToken = null;
                do
                {
                    var items = await directory.ListBlobsSegmentedAsync(blobContinuationToken, cancellationToken);
                    blobsInDirectory.AddRange(items.Results);
                }
                while (blobContinuationToken != null);

                return blobsInDirectory;
            }
        }
    }
}
