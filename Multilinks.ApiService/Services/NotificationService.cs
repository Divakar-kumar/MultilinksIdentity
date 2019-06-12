using System.Threading;
using System.Threading.Tasks;
using Multilinks.ApiService.Entities;

namespace Multilinks.ApiService.Services
{
   public class NotificationService : INotificationService
   {
      private readonly ApiServiceDbContext _context;

      public NotificationService(ApiServiceDbContext context)
      {
         _context = context;
      }

      public async Task<bool> CreateNotificationAsync(NotificationEntity notification, CancellationToken ct)
      {
         _context.Notifications.Add(notification);

         var created = await _context.SaveChangesAsync(ct);

         return created > 0 ? true : false;
      }
   }
}
