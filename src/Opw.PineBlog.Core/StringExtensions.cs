
namespace Opw.PineBlog
{
    public static class StringExtensions
    {
        public static string ToPostSlug(this string s)
        {
            return s.ToLower().ToSlug();
        }
    }
}
