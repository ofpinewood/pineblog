using System;
using System.ComponentModel.DataAnnotations;

namespace Opw.ComponentModel.DataAnnotations
{
    public class RequiredGuidAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            var valueString = value.ToString();
            if (value is Guid && valueString.Equals(Guid.Empty.ToString(), StringComparison.InvariantCultureIgnoreCase))
                return false;
            else if (value is ShortGuid && valueString.Equals(ShortGuid.Empty.ToString(), StringComparison.InvariantCultureIgnoreCase))
                return false;

            return true;
        }
    }
}
