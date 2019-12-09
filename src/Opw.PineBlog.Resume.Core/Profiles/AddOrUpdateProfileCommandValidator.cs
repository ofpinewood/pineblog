using FluentValidation;
using Opw.FluentValidation;

namespace Opw.PineBlog.Resume.Profiles
{
    /// <summary>
    /// Validator for the AddOrUpdateProfileCommand request.
    /// </summary>
    public class AddOrUpdateProfileCommandValidator : AbstractValidator<AddOrUpdateProfileCommand>
    {
        /// <summary>
        /// Implementation of AddOrUpdateProfileCommandValidator.
        /// </summary>
        public AddOrUpdateProfileCommandValidator() : base()
        {
            RuleFor(c => c.UserName).NotEmpty();
            RuleFor(c => c.Slug).IsSlug();
            //TODO: more validation
        }
    }
}
