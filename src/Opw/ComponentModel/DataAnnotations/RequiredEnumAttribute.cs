using System;
using System.ComponentModel.DataAnnotations;

namespace Opw.ComponentModel.DataAnnotations
{
    public class RequiredEnumAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || (value is Enum && value.ToString().Equals("NotSet", StringComparison.InvariantCultureIgnoreCase)))
            {
                return false;
            }

            return true;
        }
    }
}
