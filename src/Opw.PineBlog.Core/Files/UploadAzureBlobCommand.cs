using MediatR;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Files
{
    /// <summary>
    /// Command that uploads a blob to Azure blob storage.
    /// </summary>
    public class UploadAzureBlobCommand : IRequest<Result<string>>
    {
        /// <summary>
        /// The file stream of the to upload file.
        /// </summary>
        public Stream FileStream { get; set; }

        /// <summary>
        /// The target file path, excluding the file name.
        /// </summary>
        public string TargetPath { get; set; }

        /// <summary>
        /// The target file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Handler for the UploadAzureBlobCommand.
        /// </summary>
        public class Handler : IRequestHandler<UploadAzureBlobCommand, Result<string>>
        {
            private readonly CloudBlobClient _cloudBlobClient;
            private readonly IOptions<PineBlogOptions> _options;

            /// <summary>
            /// Implementation of AddPostCommand.Handler.
            /// </summary>
            /// <param name="cloudBlobClient">Cloud blob client.</param>
            /// <param name="options">The blog options.</param>
            public Handler(CloudBlobClient cloudBlobClient, IOptions<PineBlogOptions> options)
            {
                _cloudBlobClient = cloudBlobClient;
                _options = options;
            }

            /// <summary>
            /// Handle the UploadAzureBlobCommand request.
            /// </summary>
            /// <param name="request">The UploadAzureBlobCommand request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<string>> Handle(UploadAzureBlobCommand request, CancellationToken cancellationToken)
            {
                var cloudBlobContainer = await GetCloudBlobContainerAsync(cancellationToken);
                if (!cloudBlobContainer.IsSuccess)
                    return Result<string>.Fail(cloudBlobContainer.Exception);

                try
                {
                    var blobName = $"{request.TargetPath.Trim('/')}/{request.FileName.Trim('/')}";
                    var cloudBlockBlob = cloudBlobContainer.Value.GetBlockBlobReference(blobName);
                    await cloudBlockBlob.UploadFromStreamAsync(request.FileStream);

                    return Result<string>.Success(cloudBlockBlob.Uri.AbsoluteUri);
                }
                catch (Exception ex)
                {
                    return Result<string>.Fail(new FileUploadException($"The blob ({request.FileName}) upload failed", ex));
                }
            }

            private async Task<Result<CloudBlobContainer>> GetCloudBlobContainerAsync(CancellationToken cancellationToken)
            {
                var cloudBlobContainer = _cloudBlobClient.GetContainerReference(_options.Value.AzureStorageBlobContainerName);
                var created = await cloudBlobContainer.CreateIfNotExistsAsync(cancellationToken);
                if (!created)
                    return Result<CloudBlobContainer>.Fail(new FileUploadException($"Blob container does not exist and could not be created ({_options.Value.AzureStorageBlobContainerName})."));

                // Set the permissions so the blobs are public.
                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }, cancellationToken);

                return Result<CloudBlobContainer>.Success(cloudBlobContainer);
            }
        }
    }
}
