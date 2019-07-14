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
            RuleFor(c => c.AuthorId).IsRequiredGuid();
            RuleFor(c => c.Title).Length(160).NotEmpty();
            RuleFor(c => c.Description).Length(450).NotEmpty();
            RuleFor(c => c.Content).Length(2000).NotEmpty();
            //RuleFor(c => c.CoverUrl).Length(254);
            //RuleFor(c => c.CoverCaption).Length(160);
            //RuleFor(c => c.CoverLink).Length(254);
        }
    }
}
