using System.Threading;
using System.Threading.Tasks;

namespace Opw.EntityFrameworkCore
{
    public interface IEntityDbContext
    {
        Result<int> SaveChanges();
        Result<int> SaveChanges(bool acceptAllChangesOnSuccess);
        Task<Result<int>> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);
        Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken);
    }
}