using FluentValidation;

namespace Opw.PineBlog.Files
{
    /// <summary>
    /// Validator for the AddPostCommand request.
    /// </summary>
    public class UploadAzureBlobCommandValidator : AbstractValidator<UploadAzureBlobCommand>
    {
        /// <summary>
        /// Implementation of AddPostCommandValidator.
        /// </summary>
        public UploadAzureBlobCommandValidator()
        {
            RuleFor(c => c.FileName).NotEmpty();
            RuleFor(c => c.FileStream).NotEmpty();
        }
    }
}
