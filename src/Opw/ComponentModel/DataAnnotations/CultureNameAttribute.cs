using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Opw.ComponentModel.DataAnnotations
{
    public class CultureNameAttribute : StringLengthAttribute
    {
        public CultureNameAttribute() : base(5)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var baseResult = base.IsValid(value, context);
            if (baseResult != null && !string.IsNullOrEmpty(baseResult.ErrorMessage))
                return baseResult;

            if (value == null || !(value is string))
                return new ValidationResult($"Invalid culture, should be in \"en-US\" format.");

            if (!IsValidCulture(value.ToString()))
                return new ValidationResult($"Invalid culture, should be in \"en-US\" format, actual cultureName was \"{value}\".");

            return ValidationResult.Success;
        }

        bool IsValidCulture(string cultureName)
        {
            try
            {
                var culture = new CultureInfo(cultureName);
                var name = culture.EnglishName;
                if (string.IsNullOrWhiteSpace(name) || name.ToLowerInvariant().StartsWith("unknown", StringComparison.InvariantCultureIgnoreCase))
                    return false;
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
