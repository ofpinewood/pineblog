using Opw.PineBlog.Entities;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Repositories
{
    public interface IAuthorRepository
    {
        Task<Author> SingleOrDefaultAsync(Expression<Func<Author, bool>> predicate, CancellationToken cancellationToken);
    }
}