using System.Linq;

namespace Opw.PineBlog.GitDb
{
    // TODO: add tests
    internal static class PathHelper
    {
        internal static string Build(params string[] parts)
        {
            var pathParts = parts
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Select(p => p.Trim('/').Trim('\\'));
            return string.Join('/', pathParts);
        }
    }
}
