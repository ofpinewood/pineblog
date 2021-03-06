using FluentValidation.Resources;
using FluentValidation.Validators;
using Opw.PineBlog.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Opw.PineBlog.FluentValidation
{
    public class SlugValidator : PropertyValidator
    {
        public SlugValidator() : base(new SlugAttribute().ErrorMessage)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var result = new SlugAttribute().GetValidationResult(context.PropertyValue, new ValidationContext(context.Instance));

            if (result == null) return true;

            Options.ErrorMessageSource = new StaticStringSource(result.ErrorMessage);
            return false;
        }
    }
}
