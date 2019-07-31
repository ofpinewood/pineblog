using FluentValidation;

namespace Opw.PineBlog.Covers
{
    /// <summary>
    /// Validator for the AddCoverCommand request.
    /// </summary>
    public class AddCoverCommandValidator : AbstractValidator<AddCoverCommand>
    {
        /// <summary>
        /// Implementation of AddCoverCommandValidator.
        /// </summary>
        public AddCoverCommandValidator()
        {
            RuleFor(c => c.File).NotEmpty();
        }
    }
}
