
using Multilinks.ApiService.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Services
{
   public interface INotificationService
   {
      Task<bool> CreateNotificationAsync(NotificationEntity notification, CancellationToken ct);
   }
}
