using Opw.PineBlog.Entities;
using System.Threading.Tasks;
using System.Threading;
using System;
using Opw.PineBlog.Repositories;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace Opw.PineBlog.MongoDb.Repositories
{
    public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
    {
        public AuthorRepository(BlogUnitOfWork uow) : base(uow) { }

        public async Task<Author> SingleOrDefaultAsync(Expression<Func<Author, bool>> predicate, CancellationToken cancellationToken)
        {
            return await Collection
                .Find(predicate)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
