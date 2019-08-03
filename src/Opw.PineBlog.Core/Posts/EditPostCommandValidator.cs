using FluentValidation;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Validator for the IEditPostCommand requests.
    /// </summary>
    public class EditPostCommandValidator : AbstractValidator<IEditPostCommand>
    {
        /// <summary>
        /// Implementation of EditPostCommandValidator.
        /// </summary>
        public EditPostCommandValidator()
        {
            RuleFor(c => c.UserName).NotEmpty();
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
