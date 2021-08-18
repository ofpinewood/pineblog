using Microsoft.Extensions.Options;
using Opw.PineBlog.GitDb.LibGit2;
using System.Linq;

namespace Opw.PineBlog.GitDb.Repositories
{
    public abstract class RepositoryBase
    {
        protected readonly GitDbContext GitDbContext;
        protected readonly IOptions<PineBlogGitDbOptions> Options;

        public RepositoryBase(GitDbContext gitDbContext, IOptions<PineBlogGitDbOptions> options)
        {
            GitDbContext = gitDbContext;
            Options = options;
        }

        protected string BuildPath(params string[] parts)
        {
            var pathParts = parts
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Select(p => p.Trim('/').Trim('\\'));
            return string.Join('/', pathParts);
        }
    }
}
