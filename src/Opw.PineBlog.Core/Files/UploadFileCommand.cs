using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Files
{
    /// <summary>
    /// Command that uploads a cover image.
    /// </summary>
    public class UploadFileCommand : IRequest<Result>
    {
        /// <summary>
        /// The file sent with the HTTP request.
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// Handler for the UploadFileCommand.
        /// </summary>
        public class Handler : IRequestHandler<UploadFileCommand, Result>
        {
            /// <summary>
            /// Handle the UploadFileCommand request.
            /// </summary>
            /// <param name="request">The UploadFileCommand request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result> Handle(UploadFileCommand request, CancellationToken cancellationToken)
            {
                var stream = new MemoryStream();
                var result = await ProcessFormFileAsync(request.File, stream);
                if (!result.IsSuccess) return result;

                // TODO: Upload the file to azure

                return Result.Success();
            }

            private async Task<Result> ProcessFormFileAsync(IFormFile formFile, Stream targetStream)
            {
                // Use Path.GetFileName to obtain the file name, which will strip any path information passed as part of the
                // FileName property. HtmlEncode the result in case it must be returned in an error message.
                var fileName = WebUtility.HtmlEncode(Path.GetFileName(formFile.FileName));

                // TODO: add check for accepted file types
                //if (!string.Equals(formFile.ContentType, "text/plain", StringComparison.OrdinalIgnoreCase))
                //    errors.Add(formFile.Name, $"The {fieldDisplayName}file ({fileName}) must be a text file.");

                // Check the file length and don't bother attempting to read it if the file contains no content. This check
                // doesn't catch files that only have a BOM as their content, so a content length check is made later after 
                // reading the file's content to catch a file that only contains a BOM.
                if (formFile.Length == 0)
                    return Result.Fail(new FileUploadException($"The {formFile.Name}file ({fileName}) is empty."));

                if (formFile.Length > 1048576)
                    return Result.Fail(new FileUploadException($"The {formFile.Name}file ({fileName}) exceeds 1 MB."));

                try
                {
                    await formFile.OpenReadStream().CopyToAsync(targetStream);
                }
                catch (Exception ex)
                {
                    return Result.Fail(new FileUploadException($"The {formFile.Name}file ({fileName}) upload failed. Error: {ex.Message}", ex));
                }

                return Result.Success();
            }
        }
    }
}
