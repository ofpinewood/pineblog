using FluentValidation.Validators;
using Opw.ComponentModel.DataAnnotations;

namespace Opw.FluentValidation
{
    public class RequiredGuidValidator : PropertyValidator
    {
        public RequiredGuidValidator() : base(new RequiredGuidAttribute().ErrorMessage)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            return new RequiredGuidAttribute().IsValid(context.PropertyValue);
        }
    }
}
