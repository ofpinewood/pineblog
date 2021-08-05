using Opw.PineBlog.Entities;
using System.Threading.Tasks;
using System.Threading;
using System;
using Opw.PineBlog.Repositories;
using System.Linq.Expressions;
using Opw.PineBlog.Git.LibGit2;

namespace Opw.PineBlog.Git.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly GitContext _gitContext;

        public AuthorRepository(GitContext gitContext)
        {
            _gitContext = gitContext;
        }

        public async Task<Author> SingleOrDefaultAsync(Expression<Func<Author, bool>> predicate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //return await _gitContext.Authors.SingleOrDefaultAsync(predicate, cancellationToken);
        }
    }
}
