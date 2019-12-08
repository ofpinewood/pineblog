using FluentValidation;
using Opw.FluentValidation;

namespace Opw.PineBlog.Resume.Profiles
{
    /// <summary>
    /// Validator for the GetProfileQuery request.
    /// </summary>
    public class GetProfileQueryValidator : AbstractValidator<GetProfileQuery>
    {
        /// <summary>
        /// Implementation of GetProfileQueryValidator.
        /// </summary>
        public GetProfileQueryValidator()
        {
            RuleFor(c => c.Slug).IsSlug();
        }
    }
}
