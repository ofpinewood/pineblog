using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Files.Azure
{
    /// <summary>
    /// Helper for working with Azure blobs.
    /// </summary>
    public class AzureBlobHelper
    {
        private readonly CloudBlobClient _cloudBlobClient;
        private readonly IOptions<PineBlogOptions> _options;

        /// <summary>
        /// Implementation of AddPostCommand.Handler.
        /// </summary>
        /// <param name="cloudBlobClient">Cloud blob client.</param>
        /// <param name="options">The blog options.</param>
        public AzureBlobHelper(CloudBlobClient cloudBlobClient, IOptions<PineBlogOptions> options)
        {
            _cloudBlobClient = cloudBlobClient;
            _options = options;
        }

        /// <summary>
        /// Get the blob container.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task<Result<CloudBlobContainer>> GetCloudBlobContainerAsync(CancellationToken cancellationToken)
        {
            var cloudBlobContainer = _cloudBlobClient.GetContainerReference(_options.Value.AzureStorageBlobContainerName);
            await cloudBlobContainer.CreateIfNotExistsAsync(cancellationToken);

            // Set the permissions so the blobs are public.
            await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }, cancellationToken);

            return Result<CloudBlobContainer>.Success(cloudBlobContainer);
        }
    }
}
