using Opw.PineBlog.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Opw.PineBlog
{
    public static class ShortGuidExtensions
    {
        public static ShortGuid ToShortGuid(this Guid guid)
        {
            return new ShortGuid(guid);
        }
    }
}
