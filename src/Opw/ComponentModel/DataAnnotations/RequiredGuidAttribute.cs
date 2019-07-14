using System;
using System.ComponentModel.DataAnnotations;

namespace Opw.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Specifies that a Guid field value is required.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class RequiredGuidAttribute : RequiredAttribute
    {
        /// <summary>
        /// Checks that the value of the required data field is not empty.
        /// </summary>
        /// <param name="value">The data field value to validate.</param>
        /// <returns>true if validation is successful; otherwise, false.</returns>
        /// <exception cref="ValidationException">The data field value was null.</exception>
        public override bool IsValid(object value)
        {
            if (value == null)
                return base.IsValid(value);

            var valueString = value.ToString();
            if (value is Guid && valueString.Equals(Guid.Empty.ToString(), StringComparison.InvariantCultureIgnoreCase))
                return false;
            else if (value is ShortGuid && valueString.Equals(ShortGuid.Empty.ToString(), StringComparison.InvariantCultureIgnoreCase))
                return false;

            return true;
        }
    }
}
