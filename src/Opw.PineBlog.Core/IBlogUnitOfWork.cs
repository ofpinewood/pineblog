using Opw.PineBlog.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog
{
    public interface IBlogUnitOfWork
    {
        IBlogSettingsRepository BlogSettings { get; }
        IAuthorRepository Authors { get; }
        IPostRepository Posts { get; }

        Result<int> SaveChanges();
        Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken);
    }
}