using Opw.PineBlog.Entities;
using System.Threading.Tasks;
using System.Threading;
using System;
using Opw.PineBlog.Repositories;
using System.Linq.Expressions;
using Opw.PineBlog.GitDb.LibGit2;

namespace Opw.PineBlog.GitDb.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly GitDbContext _gitDbContext;

        public AuthorRepository(GitDbContext gitDbContext)
        {
            _gitDbContext = gitDbContext;
        }

        public async Task<Author> SingleOrDefaultAsync(Expression<Func<Author, bool>> predicate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //return await _gitContext.Authors.SingleOrDefaultAsync(predicate, cancellationToken);
        }
    }
}
