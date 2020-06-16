using System.Threading.Tasks;
using System.Threading;
using Opw.PineBlog.Repositories;
using Opw.PineBlog.MongoDb.Repositories;
using MongoDB.Driver;

namespace Opw.PineBlog.MongoDb
{
    public class BlogUnitOfWork : IBlogUnitOfWork
    {
        internal readonly IMongoDatabase Database;
        internal int SaveChangeCount;

        public IBlogSettingsRepository BlogSettings { get; }
        public IAuthorRepository Authors { get; }
        public IPostRepository Posts { get; }

        public BlogUnitOfWork(IMongoDatabase database)
        {
            Database = database;

            BlogSettings = new BlogSettingsRepository(this);
            Authors = new AuthorRepository(this);
            Posts = new PostRepository(this);
        }

        public Result<int> SaveChanges()
        {
            var result = Result<int>.Success(SaveChangeCount);
            SaveChangeCount = 0;
            return result;
        }

        public Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var result = Result<int>.Success(SaveChangeCount);
            SaveChangeCount = 0;
            return Task.FromResult(result);
        }
    }
}
