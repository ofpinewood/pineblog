using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace Opw.PineBlog.Files.AzureBlobs
{
    /// <summary>
    /// Command that uploads a blob to Azure blob storage.
    /// </summary>
    public class UploadAzureBlobCommand : IUploadFileCommand
    {
        /// <summary>
        /// The file sent with the HTTP request.
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// Allowed file type.
        /// </summary>
        public FileType AllowedFileType { get; set; }

        /// <summary>
        /// The target file path, excluding the file name.
        /// </summary>
        public string TargetPath { get; set; }

        /// <summary>
        /// Handler for the UploadAzureBlobCommand.
        /// </summary>
        public class Handler : IUploadFileCommandHandler<UploadAzureBlobCommand>
        {
            private readonly AzureBlobHelper _azureBlobHelper;

            /// <summary>
            /// Implementation of UploadAzureBlobCommand.Handler.
            /// </summary>
            /// <param name="azureBlobHelper">Azure blob helper.</param>
            public Handler(AzureBlobHelper azureBlobHelper)
            {
                _azureBlobHelper = azureBlobHelper;
            }

            /// <summary>
            /// Handle the UploadAzureBlobCommand request.
            /// </summary>
            /// <param name="request">The UploadAzureBlobCommand request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<string>> Handle(UploadAzureBlobCommand request, CancellationToken cancellationToken)
            {
                // Use Path.GetFileName to obtain the file name, which will strip any path information passed as part of the FileName property.
                var fileName = Path.GetFileName(request.File.FileName);
                // Make the filename url safe
                fileName = fileName.ToUrlSafeFileName();

                var stream = new MemoryStream();
                var result = await ProcessFormFileAsync(request.File, fileName, stream);
                if (!result.IsSuccess) return result;

                var blobContainerClient = await _azureBlobHelper.GetBlobContainerClientAsync(cancellationToken);
                if (!blobContainerClient.IsSuccess)
                    return Result<string>.Fail(blobContainerClient.Exception);

                try
                {
                    var blobName = $"{request.TargetPath.Trim('/')}/{fileName.Trim('/')}";
                    var blobClient = blobContainerClient.Value.GetBlobClient(blobName);

                    stream.Position = 0;
                    var response = await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = fileName.GetMimeType() } }, cancellationToken);
                    
                    return Result<string>.Success(blobClient.Uri.AbsoluteUri);
                }
                catch (Exception ex)
                {
                    return Result<string>.Fail(new FileUploadException($"The blob ({fileName}) upload failed", ex));
                }
            }

            private async Task<Result<string>> ProcessFormFileAsync(IFormFile formFile, string fileName, Stream targetStream)
            {
                try
                {
                    await formFile.OpenReadStream().CopyToAsync(targetStream);
                }
                catch (Exception ex)
                {
                    return Result<string>.Fail(new FileUploadException($"The {formFile.Name} file ({fileName}) upload failed. Error: {ex.Message}", ex));
                }

                return Result<string>.Success();
            }
        }
    }
}
