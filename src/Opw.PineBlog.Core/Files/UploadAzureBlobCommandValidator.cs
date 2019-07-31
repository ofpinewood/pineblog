using FluentValidation;

namespace Opw.PineBlog.Files
{
    /// <summary>
    /// Validator for the UploadAzureBlobCommand request.
    /// </summary>
    public class UploadAzureBlobCommandValidator : AbstractValidator<UploadAzureBlobCommand>
    {
        /// <summary>
        /// Implementation of UploadAzureBlobCommandValidator.
        /// </summary>
        public UploadAzureBlobCommandValidator()
        {
            RuleFor(c => c.FileName).NotEmpty();
            RuleFor(c => c.FileStream).NotEmpty();
        }
    }
}
