using FluentValidation;
using Opw.FluentValidation;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Validator GetPostQuery request.
    /// </summary>
    public class GetPostQueryValidator : AbstractValidator<GetPostQuery>
    {
        /// <summary>
        /// Implementation of GetPostQueryValidator.
        /// </summary>
        public GetPostQueryValidator()
        {
            RuleFor(c => c.Slug).IsSlug();
        }
    }
}
