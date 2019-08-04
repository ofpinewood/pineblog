using Opw.FluentValidation;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Validator for the AddPostCommand request.
    /// </summary>
    public class UpdatePostCommandValidator : EditPostCommandValidator<UpdatePostCommand>
    {
        /// <summary>
        /// Implementation of AddPostCommandValidator.
        /// </summary>
        public UpdatePostCommandValidator() : base()
        {
            RuleFor(c => c.Id).IsRequiredGuid();
        }
    }
}
