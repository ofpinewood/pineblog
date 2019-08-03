using MediatR;
using Microsoft.AspNetCore.Http;
using Opw.PineBlog.Files.Azure;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Files
{
    /// <summary>
    /// Command that uploads a file.
    /// </summary>
    public class UploadFileCommand : IRequest<Result<string>>
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
        /// The file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The target file path, excluding the file name.
        /// </summary>
        public string TargetPath { get; set; }

        /// <summary>
        /// Handler for the UploadFileCommand.
        /// </summary>
        public class Handler : IRequestHandler<UploadFileCommand, Result<string>>
        {
            private readonly IMediator _mediator;

            /// <summary>
            /// Implementation of UploadFileCommand.Handler.
            /// </summary>
            /// <param name="mediator">Mediator</param>
            public Handler(IMediator mediator)
            {
                _mediator = mediator;
            }

            /// <summary>
            /// Handle the UploadFileCommand request.
            /// </summary>
            /// <param name="request">The UploadFileCommand request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<string>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
            {
                var stream = new MemoryStream();
                var result = await ProcessFormFileAsync(request.File, stream);
                if (!result.IsSuccess) return result;

                // TODO: make the upload target configurable (not only azure blob storage)
                // TODO: check if we are not overwriting files, overwriting should not be possible
                result = await _mediator.Send(new UploadAzureBlobCommand {
                    FileStream = stream,
                    FileName = !string.IsNullOrWhiteSpace(request.FileName) ? request.FileName : request.File.FileName,
                    TargetPath = request.TargetPath
                }, cancellationToken);

                return result;
            }

            private async Task<Result<string>> ProcessFormFileAsync(IFormFile formFile, Stream targetStream)
            {
                // Use Path.GetFileName to obtain the file name, which will strip any path information passed as part of the FileName property.
                var fileName = Path.GetFileName(formFile.FileName);

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
