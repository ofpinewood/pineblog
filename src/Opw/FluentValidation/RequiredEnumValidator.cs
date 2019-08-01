using FluentValidation.Resources;
using FluentValidation.Validators;
using Opw.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Opw.FluentValidation
{
    public class RequiredEnumValidator : PropertyValidator
    {
        public RequiredEnumValidator() : base(new RequiredEnumAttribute().ErrorMessage)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var result = new RequiredEnumAttribute().GetValidationResult(context.PropertyValue, new ValidationContext(context.Instance));

            if (result == null) return true;

            Options.ErrorMessageSource = new StaticStringSource(result.ErrorMessage);
            return false;
        }
    }
}
