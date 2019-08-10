using FluentValidation;
using Opw.FluentValidation;
using System.IO;

namespace Opw.PineBlog.Files
{
    /// <summary>
    /// Base class for validators for UploadFileCommand requests.
    /// </summary>
    public abstract class AbstractUploadFileCommandValidator<TRequest> : AbstractValidator<TRequest>
        where TRequest : IUploadFileCommand
    {
        /// <summary>
        /// Implementation of AbstractUploadFileCommandValidator.
        /// </summary>
        public AbstractUploadFileCommandValidator()
        {
            RuleFor(c => c.File).NotEmpty();
            RuleFor(c => c.AllowedFileType).IsRequiredEnum();

            RuleFor(c => c).Custom((request, context) =>
            {
                if (request.File?.FileName == null || (int)request.AllowedFileType < 1)
                    return;

                // Use Path.GetFileName to obtain the file name, which will strip any path information passed as part of the
                // FileName property.
                var fileName = Path.GetFileName(request.File.FileName);

                if (!request.AllowedFileType.IsFileTypeSupported(fileName.GetMimeType()))
                    context.AddFailure(nameof(request.File), $"The {request.File.Name} file ({fileName}) must be of type \"{request.AllowedFileType.ToString().ToLowerInvariant()}\".");
            });

            RuleFor(c => c).Custom((request, context) =>
            {
                if (request.File?.FileName == null)
                    return;

                // Use Path.GetFileName to obtain the file name, which will strip any path information passed as part of the
                // FileName property.
                var fileName = Path.GetFileName(request.File.FileName);

                // Check the file length and don't bother attempting to read it if the file contains no content. This check
                // doesn't catch files that only have a BOM as their content, so a content length check is made later after 
                // reading the file's content to catch a file that only contains a BOM.
                if (request.File.Length == 0)
                    context.AddFailure(nameof(request.File), $"The {request.File.Name} file ({fileName}) is empty.");

                if (request.File.Length > 1048576)
                    context.AddFailure(nameof(request.File), $"The {request.File.Name} file ({fileName}) exceeds 1 MB.");
            });
        }
    }
}
