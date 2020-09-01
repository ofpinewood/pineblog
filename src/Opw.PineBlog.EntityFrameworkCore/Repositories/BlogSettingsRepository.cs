using Opw.PineBlog.Entities;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using Opw.PineBlog.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Opw.PineBlog.EntityFrameworkCore.Repositories
{
    public class BlogSettingsRepository : IBlogSettingsRepository
    {
        private readonly BlogEntityDbContext _dbContext;

        public BlogSettingsRepository(BlogEntityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BlogSettings> SingleOrDefaultAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.BlogSettings.SingleOrDefaultAsync(cancellationToken);
        }

        public BlogSettings Add([NotNull] BlogSettings blogSettings)
        {
            var entry = _dbContext.BlogSettings.Add(blogSettings);
            return entry.Entity;
        }

        public BlogSettings Update([NotNull] BlogSettings blogSettings)
        {
            var entry = _dbContext.BlogSettings.Update(blogSettings);
            return entry.Entity;
        }
    }
}
