
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Files.AzureBlobs
{
    /// <summary>
    /// Helper for working with Azure blobs.
    /// </summary>
    public class AzureBlobHelper
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IOptions<PineBlogOptions> _options;

        /// <summary>
        /// Implementation of AddPostCommand.Handler.
        /// </summary>
        /// <param name="blobServiceClient">Blob service client.</param>
        /// <param name="options">The blog options.</param>
        public AzureBlobHelper(BlobServiceClient blobServiceClient, IOptions<PineBlogOptions> options)
        {
            _blobServiceClient = blobServiceClient;
            _options = options;
        }

        /// <summary>
        /// Get the blob container client.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task<Result<BlobContainerClient>> GetBlobContainerClientAsync(CancellationToken cancellationToken)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_options.Value.AzureStorageBlobContainerName);
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            return Result<BlobContainerClient>.Success(blobContainerClient);
        }
    }
}
