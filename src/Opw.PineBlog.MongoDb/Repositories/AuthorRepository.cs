using Opw.PineBlog.Entities;
using System.Threading.Tasks;
using System.Threading;
using System;
using Opw.PineBlog.Repositories;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace Opw.PineBlog.MongoDb.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BlogUnitOfWork _uow;
        private readonly IMongoCollection<Author> _collection;

        public AuthorRepository(BlogUnitOfWork uow)
        {
            _uow = uow;
            _collection = _uow.Database.GetCollection<Author>($"{nameof(Author)}s");
        }

        public async Task<Author> SingleOrDefaultAsync(Expression<Func<Author, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _collection
                .Find(predicate)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
