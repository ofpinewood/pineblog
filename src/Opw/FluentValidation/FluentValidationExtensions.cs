using FluentValidation;

namespace Opw.FluentValidation
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, Guid> IsRequiredGuid<T, Guid>(this IRuleBuilder<T, Guid> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new RequiredGuidValidator());
        }

        public static IRuleBuilderOptions<T, TProperty> IsRequiredEnum<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new RequiredEnumValidator());
        }

        public static IRuleBuilderOptions<T, string> IsSlug<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new SlugValidator());
        }

        public static IRuleBuilderOptions<T, string> IsCultureName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new CultureNameValidator());
        }
    }
}
