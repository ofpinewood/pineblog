using Opw.PineBlog.Entities;
using System.Threading.Tasks;
using System.Threading;
using System;
using Opw.PineBlog.Repositories;
using System.Linq.Expressions;
using Microsoft.Extensions.Options;

namespace Opw.PineBlog.GitDb.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly IOptionsSnapshot<PineBlogGitDbOptions> _options;

        public AuthorRepository(IOptionsSnapshot<PineBlogGitDbOptions> options)
        {
            _options = options;
        }

        public async Task<Author> SingleOrDefaultAsync(Expression<Func<Author, bool>> predicate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //return await _gitContext.Authors.SingleOrDefaultAsync(predicate, cancellationToken);
        }
    }
}
