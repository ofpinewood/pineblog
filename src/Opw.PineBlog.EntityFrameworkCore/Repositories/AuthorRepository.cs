using Opw.PineBlog.Entities;
using System.Threading.Tasks;
using System.Threading;
using System;
using Opw.PineBlog.Repositories;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Opw.PineBlog.EntityFrameworkCore.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BlogEntityDbContext _dbContext;

        public AuthorRepository(BlogEntityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Author> SingleOrDefaultAsync(Expression<Func<Author, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbContext.Authors.SingleOrDefaultAsync(predicate, cancellationToken);
        }
    }
}
