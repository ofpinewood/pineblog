using FluentValidation;

namespace Opw.PineBlog.Blogs
{
    /// <summary>
    /// Validator for the UpdateBlogSettingsCommand request.
    /// </summary>
    public class UpdateBlogSettingsCommandValidator : AbstractValidator<UpdateBlogSettingsCommand>
    {
        /// <summary>
        /// Implementation of UpdateBlogSettingsCommandValidator.
        /// </summary>
        public UpdateBlogSettingsCommandValidator()
        {
            RuleFor(c => c.Title).MaximumLength(160).NotEmpty();
            RuleFor(c => c.Description).MaximumLength(450);
            RuleFor(c => c.CoverUrl).MaximumLength(254);
            RuleFor(c => c.CoverCaption).MaximumLength(160);
            RuleFor(c => c.CoverLink).MaximumLength(254);
        }
    }
}
