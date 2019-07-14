using FluentValidation.Resources;
using FluentValidation.Validators;
using Opw.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Opw.FluentValidation
{
    public class RequiredGuidValidator : PropertyValidator
    {
        public RequiredGuidValidator() : base(new RequiredGuidAttribute().ErrorMessage)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var result = new RequiredGuidAttribute().GetValidationResult(context.PropertyValue, new ValidationContext(context.Instance));

            if (result == null) return true;

            Options.ErrorMessageSource = new StaticStringSource(result.ErrorMessage);
            return false;
        }
    }
}
