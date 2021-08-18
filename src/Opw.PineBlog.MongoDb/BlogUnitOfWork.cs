using System.Threading.Tasks;
using System.Threading;
using Opw.PineBlog.Repositories;
using Opw.PineBlog.MongoDb.Repositories;
using MongoDB.Driver;
using MongoDB.Bson;
using System;

namespace Opw.PineBlog.MongoDb
{
    public class BlogUnitOfWork : IBlogUnitOfWork, IDisposable
    {
        private IChangeStreamCursor<ChangeStreamDocument<BsonDocument>> _changeStreamCursor;

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

            _changeStreamCursor = Database.Watch();
            _changeStreamCursor.ForEachAsync((changeStreamDocument) => NotifyEntityChangeWatchers(changeStreamDocument));
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

        private void NotifyEntityChangeWatchers(ChangeStreamDocument<BsonDocument> changeStreamDocument)
        {
            DocumentChangeObserver.Instance.OnChanged(new DocumentChangeEventArgs(changeStreamDocument));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _changeStreamCursor?.Dispose();
                _changeStreamCursor = null;
                GC.Collect();
            }
        }
    }
}
