using FluentValidation.Validators;
using Opw.ComponentModel.DataAnnotations;

namespace Opw.FluentValidation
{
    public class RequiredEnumValidator : PropertyValidator
    {
        public RequiredEnumValidator() : base(new RequiredEnumAttribute().ErrorMessage)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            return new RequiredEnumAttribute().IsValid(context.PropertyValue);
        }
    }
}
