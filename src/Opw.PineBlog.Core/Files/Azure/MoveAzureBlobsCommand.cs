using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Files.Azure
{
    /// <summary>
    /// Command that moves blobs in Azure blob storage.
    /// </summary>
    public class MoveAzureBlobsCommand : IMoveFilesCommand
    {
        /// <summary>
        /// The source folder path.
        /// </summary>
        public string SourcePath { get; set; }

        /// <summary>
        /// The target folder path.
        /// </summary>
        public string TargetPath { get; set; }

        /// <summary>
        /// Handler for the MoveAzureBlobsCommand.
        /// </summary>
        public class Handler : IMoveFilesCommandHandler<MoveAzureBlobsCommand>
        {
            private readonly AzureBlobHelper _azureBlobHelper;

            /// <summary>
            /// Implementation of MoveAzureBlobsCommand.Handler.
            /// </summary>
            /// <param name="azureBlobHelper">Azure blob helper.</param>
            public Handler(AzureBlobHelper azureBlobHelper)
            {
                _azureBlobHelper = azureBlobHelper;
            }

            /// <summary>
            /// Handle the MoveAzureBlobsCommand request.
            /// </summary>
            /// <param name="request">The MoveAzureBlobsCommand request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<string>> Handle(MoveAzureBlobsCommand request, CancellationToken cancellationToken)
            {
                var cloudBlobContainer = await _azureBlobHelper.GetCloudBlobContainerAsync(cancellationToken);
                if (!cloudBlobContainer.IsSuccess)
                    return Result<string>.Fail(cloudBlobContainer.Exception);

                try
                {
                    var source = (CloudBlockBlob)await cloudBlobContainer.Value.GetBlobReferenceFromServerAsync(request.SourcePath);
                    var target = cloudBlobContainer.Value.GetBlockBlobReference(request.TargetPath);

                    var result = await target.StartCopyAsync(source);

                    while (target.CopyState.Status == CopyStatus.Pending)
                        await Task.Delay(100);

                    if (target.CopyState.Status != CopyStatus.Success)
                        throw new Exception("Copy status: " + target.CopyState.Status);

                    var sourceFiles = await ListAsync(cloudBlobContainer.Value.GetDirectoryReference(request.SourcePath), cancellationToken);
                    var targetFiles = await ListAsync(cloudBlobContainer.Value.GetDirectoryReference(request.TargetPath), cancellationToken);
                    if (sourceFiles.Count() != targetFiles.Count())
                        throw new Exception("Some files were not moved.");

                    await source.DeleteAsync();
                    return Result<string>.Success(target.Uri.AbsoluteUri);
                }
                catch (Exception ex)
                {
                    return Result<string>.Fail(new FileUploadException("Move failed", ex));
                }
            }

            // TODO: duplicate code move to helper and add tests
            private async Task<IEnumerable<IListBlobItem>> ListAsync(CloudBlobDirectory directory, CancellationToken cancellationToken)
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
