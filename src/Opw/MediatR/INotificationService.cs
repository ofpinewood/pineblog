using System.Threading.Tasks;

namespace Opw.MediatR
{
    public interface INotificationService
    {
        Task SendAsync<T>(Result<T> result);
    }
}
