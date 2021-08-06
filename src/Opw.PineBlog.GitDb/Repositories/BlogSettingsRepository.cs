using Opw.PineBlog.Entities;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using Opw.PineBlog.Repositories;
using System;
using Opw.PineBlog.GitDb.LibGit2;

namespace Opw.PineBlog.GitDb.Repositories
{
    public class BlogSettingsRepository : IBlogSettingsRepository
    {
        private readonly GitDbContext _gitDbContext;

        public BlogSettingsRepository(GitDbContext gitDbContext)
        {
            _gitDbContext = gitDbContext;
        }

        public async Task<BlogSettings> SingleOrDefaultAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //return await _dbContext.BlogSettings.SingleOrDefaultAsync(cancellationToken);
        }

        public BlogSettings Add([NotNull] BlogSettings blogSettings)
        {
            throw new NotImplementedException();
        }

        public BlogSettings Update([NotNull] BlogSettings blogSettings)
        {
            throw new NotImplementedException();
        }
    }
}
