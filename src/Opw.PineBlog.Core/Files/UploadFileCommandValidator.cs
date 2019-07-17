using FluentValidation;

namespace Opw.PineBlog.Files
{
    /// <summary>
    /// Validator for the AddPostCommand request.
    /// </summary>
    public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
    {
        /// <summary>
        /// Implementation of AddPostCommandValidator.
        /// </summary>
        public UploadFileCommandValidator()
        {
            RuleFor(c => c.File).NotEmpty();
        }
    }
}
