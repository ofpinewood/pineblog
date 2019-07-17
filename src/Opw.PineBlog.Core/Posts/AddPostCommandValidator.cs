using FluentValidation;
using Opw.FluentValidation;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Validator for the AddPostCommand request.
    /// </summary>
    public class AddPostCommandValidator : AbstractValidator<AddPostCommand>
    {
        /// <summary>
        /// Implementation of AddPostCommandValidator.
        /// </summary>
        public AddPostCommandValidator()
        {
            RuleFor(c => c.UserName).NotEmpty();
            RuleFor(c => c.Title).MaximumLength(160).NotEmpty();
            RuleFor(c => c.Description).MaximumLength(450).NotEmpty();
            RuleFor(c => c.Categories).MaximumLength(2000);
            RuleFor(c => c.Content).NotEmpty();
            //RuleFor(c => c.CoverUrl).Length(254);
            //RuleFor(c => c.CoverCaption).Length(160);
            //RuleFor(c => c.CoverLink).Length(254);
        }
    }
}
