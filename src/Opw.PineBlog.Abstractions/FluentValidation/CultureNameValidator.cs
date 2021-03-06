using FluentValidation.Validators;
using Opw.PineBlog.ComponentModel.DataAnnotations;

namespace Opw.PineBlog.FluentValidation
{
    public class CultureNameValidator : PropertyValidator
    {
        public CultureNameValidator() : base(new CultureNameAttribute().ErrorMessage)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            return new CultureNameAttribute().IsValid(context.PropertyValue);
        }
    }
}
