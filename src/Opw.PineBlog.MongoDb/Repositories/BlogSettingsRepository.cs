using Opw.PineBlog.Entities;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using Opw.PineBlog.Repositories;
using MongoDB.Driver;

namespace Opw.PineBlog.MongoDb.Repositories
{
    public class BlogSettingsRepository : RepositoryBase<BlogSettings>, IBlogSettingsRepository
    {
        public BlogSettingsRepository(BlogUnitOfWork uow) : base(uow) { }

        public async Task<BlogSettings> SingleOrDefaultAsync(CancellationToken cancellationToken)
        {
            return await Collection
                .Find(Builders<BlogSettings>.Filter.Empty)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public new BlogSettings Add([NotNull] BlogSettings blogSettings)
        {
            return base.Add(blogSettings);
        }

        public BlogSettings Update([NotNull] BlogSettings blogSettings)
        {
            return Update(_ => true, blogSettings);
        }
    }
}
