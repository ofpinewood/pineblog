using FluentValidation;
using Opw.FluentValidation;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Validator for the PublishPostCommand request.
    /// </summary>
    public class PublishPostCommandValidator : AbstractValidator<PublishPostCommand>
    {
        /// <summary>
        /// Implementation of PublishPostCommandValidator.
        /// </summary>
        public PublishPostCommandValidator()
        {
            RuleFor(c => c.Id).IsRequiredGuid();
        }
    }
}
