using Opw.PineBlog.Entities;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Repositories
{
    public interface IBlogSettingsRepository
    {
        Task<BlogSettings> SingleOrDefaultAsync(CancellationToken cancellationToken);

        BlogSettings Add([NotNull] BlogSettings blogSettings);

        BlogSettings Update([NotNull] BlogSettings blogSettings);
    }
}