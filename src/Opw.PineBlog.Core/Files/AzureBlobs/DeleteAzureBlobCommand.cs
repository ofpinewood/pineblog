using System;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Files.AzureBlobs
{
    /// <summary>
    /// Command that deletes a blob to Azure blob storage.
    /// </summary>
    public class DeleteAzureBlobCommand : IDeleteFileCommand
    {
        /// <summary>
        /// The name of the file to delete.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The target file path, excluding the file name.
        /// </summary>
        public string TargetPath { get; set; }

        /// <summary>
        /// Handler for the DeleteAzureBlobCommand.
        /// </summary>
        public class Handler : IDeleteFileCommandHandler<DeleteAzureBlobCommand>
        {
            private readonly AzureBlobHelper _azureBlobHelper;

            /// <summary>
            /// Implementation of DeleteAzureBlobCommand.Handler.
            /// </summary>
            /// <param name="azureBlobHelper">Azure blob helper.</param>
            public Handler(AzureBlobHelper azureBlobHelper)
            {
                _azureBlobHelper = azureBlobHelper;
            }

            /// <summary>
            /// Handle the DeleteAzureBlobCommand request.
            /// </summary>
            /// <param name="request">The DeleteAzureBlobCommand request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result> Handle(DeleteAzureBlobCommand request, CancellationToken cancellationToken)
            {
                var blobContainerClient = await _azureBlobHelper.GetBlobContainerClientAsync(cancellationToken);
                if (!blobContainerClient.IsSuccess)
                    return Result.Fail(blobContainerClient.Exception);

                try
                {
                    var blobName = $"{request.TargetPath.Trim('/')}/{request.FileName.Trim('/')}";
                    var result = await blobContainerClient.Value.DeleteBlobIfExistsAsync(blobName);
                    if (!result)
                        return Result.Fail(new FileDeleteException($"The blob ({request.FileName}) does not exist."));

                    return Result.Success();
                }
                catch (Exception ex)
                {
                    return Result.Fail(new FileDeleteException($"The blob ({request.FileName}) could not be deleted.", ex));
                }
            }
        }
    }
}
