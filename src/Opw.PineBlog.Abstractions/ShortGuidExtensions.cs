using System;

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
