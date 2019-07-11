using FluentValidation;
using Opw.FluentValidation;

namespace Opw.PineBlog.Posts
{
    public class GetPostQueryValidator : AbstractValidator<GetPostQuery>
    {
        public GetPostQueryValidator()
        {
            RuleFor(c => c.Slug).IsSlug();
        }
    }
}
