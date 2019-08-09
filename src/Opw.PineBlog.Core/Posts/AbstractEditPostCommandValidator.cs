using FluentValidation;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Base class for validators for the IEditPostCommand requests.
    /// </summary>
    public abstract class AbstractEditPostCommandValidator<TRequest> : AbstractValidator<TRequest>
        where TRequest : IEditPostCommand
    {
        /// <summary>
        /// Implementation of AbstractEditPostCommandValidator.
        /// </summary>
        public AbstractEditPostCommandValidator()
        {
            RuleFor(c => c.Title).MaximumLength(160).NotEmpty();
            RuleFor(c => c.Description).MaximumLength(450);
            RuleFor(c => c.Categories).MaximumLength(2000);
            RuleFor(c => c.Content).NotEmpty();
            RuleFor(c => c.CoverUrl).MaximumLength(254);
            RuleFor(c => c.CoverCaption).MaximumLength(160);
            RuleFor(c => c.CoverLink).MaximumLength(254);
        }
    }
}
