using FluentValidation;
using Opw.PineBlog.FluentValidation;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Validator for the UnpublishPostCommand request.
    /// </summary>
    public class UnpublishPostCommandValidator : AbstractValidator<UnpublishPostCommand>
    {
        /// <summary>
        /// Implementation of UnpublishPostCommandValidator.
        /// </summary>
        public UnpublishPostCommandValidator()
        {
            RuleFor(c => c.Id).IsRequiredGuid();
        }
    }
}
