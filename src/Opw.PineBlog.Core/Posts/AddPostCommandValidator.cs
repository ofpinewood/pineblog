using FluentValidation;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Validator for the AddPostCommand request.
    /// </summary>
    public class AddPostCommandValidator : EditPostCommandValidator<AddPostCommand>
    {
        /// <summary>
        /// Implementation of AddPostCommandValidator.
        /// </summary>
        public AddPostCommandValidator() : base()
        {
            RuleFor(c => c.UserName).NotEmpty();
        }
    }
}
