using System;
using System.ComponentModel.DataAnnotations;

namespace Opw.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Specifies that a data field value is a valid slug.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class SlugAttribute : ValidationAttribute
    {
        /// <summary>
        /// Checks that the value of the data field is a valid slug.
        /// </summary>
        /// <param name="value">The data field value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>true if validation is successful; otherwise, false.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            if (!(value is string)) return new ValidationResult("Value not a string");
            if (string.IsNullOrWhiteSpace(value.ToString())) return new ValidationResult("Value empty");

            var memberNames = new string[0];
            if (validationContext != null)
                memberNames = new[] { validationContext.MemberName };

            string s = value.ToString();

            if (s.Length > 160) return new ValidationResult("Invalid slug, maximum length is 160.", memberNames);

            var slug = s.ToSlug();
            if (!s.Equals(slug)) return new ValidationResult($"Invalid slug, an alternative would be \"{slug}\"", memberNames);

            return ValidationResult.Success;
        }
    }
}
