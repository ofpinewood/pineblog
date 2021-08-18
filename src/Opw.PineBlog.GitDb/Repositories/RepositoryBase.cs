using Microsoft.Extensions.Options;
using Opw.PineBlog.GitDb.LibGit2;

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
    }
}
