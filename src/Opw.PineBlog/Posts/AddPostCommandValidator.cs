using FluentValidation;
using Opw.FluentValidation;

namespace Opw.PineBlog.Posts
{
    public class AddPostCommandValidator : AbstractValidator<AddPostCommand>
    {
        public AddPostCommandValidator()
        {
            RuleFor(c => c.AuthorId).IsRequiredGuid();
            RuleFor(c => c.Title).Length(160).NotEmpty();
            RuleFor(c => c.Description).Length(450).NotEmpty();
            RuleFor(c => c.Content).Length(2000).NotEmpty();
            RuleFor(c => c.CoverUrl).Length(254);
            RuleFor(c => c.CoverCaption).Length(160);
            RuleFor(c => c.CoverLink).Length(254);
        }
    }
}
