using FluentValidation;

namespace Opw.PineBlog.Files
{
    /// <summary>
    /// Validator for the UploadFileCommand request.
    /// </summary>
    public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
    {
        /// <summary>
        /// Implementation of UploadFileCommandValidator.
        /// </summary>
        public UploadFileCommandValidator()
        {
            RuleFor(c => c.File).NotEmpty();
        }
    }
}
