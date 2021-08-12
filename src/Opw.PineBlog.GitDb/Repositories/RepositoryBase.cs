using Microsoft.Extensions.Options;
using Opw.PineBlog.GitDb.LibGit2;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.GitDb.Repositories
{
    public abstract class RepositoryBase
    {
        protected readonly IOptionsSnapshot<PineBlogGitDbOptions> Options;

        public RepositoryBase(IOptionsSnapshot<PineBlogGitDbOptions> options)
        {
            Options = options;
        }

        /// <summary>
        /// Get the GitDb context with the correct branch checked out.
        /// </summary>
        protected async Task<GitDbContext> GetGitDbContextAsync(CancellationToken cancellationToken)
        {
            var gitDbContext = await GitDbContext.CreateAsync(Options.Value, cancellationToken);
            await gitDbContext.CheckoutBranchAsync(Options.Value.Branch, cancellationToken);
            return gitDbContext;
        }
    }
}
