using FluentValidation;
using Opw.PineBlog.FluentValidation;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Validator for the DeletePostCommand request.
    /// </summary>
    public class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
    {
        /// <summary>
        /// Implementation of DeletePostCommandValidator.
        /// </summary>
        public DeletePostCommandValidator()
        {
            RuleFor(c => c.Id).IsRequiredGuid();
        }
    }
}
