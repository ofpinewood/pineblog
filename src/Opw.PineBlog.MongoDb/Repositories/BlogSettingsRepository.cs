using Opw.PineBlog.Entities;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using Opw.PineBlog.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Opw.PineBlog.MongoDb.Repositories
{
    public class BlogSettingsRepository : IBlogSettingsRepository
    {
        private readonly BlogUnitOfWork _uow;
        private readonly IMongoCollection<BlogSettings> _collection;

        public BlogSettingsRepository(BlogUnitOfWork uow)
        {
            _uow = uow;
            _collection = _uow.Database.GetCollection<BlogSettings>(nameof(BlogSettings));
        }

        public async Task<BlogSettings> SingleOrDefaultAsync(CancellationToken cancellationToken)
        {
            return await _collection
                .Find(_ => true)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public BlogSettings Add([NotNull] BlogSettings blogSettings)
        {
            _collection.InsertOne(blogSettings);
            _uow.SaveChangeCount++;
            return blogSettings;
        }

        public BlogSettings Update([NotNull] BlogSettings blogSettings)
        {
            _collection.ReplaceOne(_ => true, blogSettings);
            _uow.SaveChangeCount++;
            return blogSettings;
        }
    }
}
