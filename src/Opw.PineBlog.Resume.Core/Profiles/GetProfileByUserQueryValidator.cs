using FluentValidation;
using Opw.FluentValidation;

namespace Opw.PineBlog.Resume.Profiles
{
    /// <summary>
    /// Validator for the GetProfileByUserQuery request.
    /// </summary>
    public class GetProfileByUserQueryValidator : AbstractValidator<GetProfileByUserQuery>
    {
        /// <summary>
        /// Implementation of GetProfileByUserQueryValidator.
        /// </summary>
        public GetProfileByUserQueryValidator()
        {
            RuleFor(c => c.UserName).IsSlug();
        }
    }
}
