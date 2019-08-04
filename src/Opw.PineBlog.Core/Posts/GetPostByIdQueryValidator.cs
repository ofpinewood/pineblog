using FluentValidation;
using Opw.FluentValidation;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Validator for the GetPostByIdQuery request.
    /// </summary>
    public class GetPostByIdQueryValidator : AbstractValidator<GetPostByIdQuery>
    {
        /// <summary>
        /// Implementation of GetPostByIdQueryValidator.
        /// </summary>
        public GetPostByIdQueryValidator()
        {
            RuleFor(c => c.Id).IsRequiredGuid();
        }
    }
}
