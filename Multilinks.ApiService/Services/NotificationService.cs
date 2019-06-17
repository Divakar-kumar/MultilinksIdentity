using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Multilinks.ApiService.Entities;
using Multilinks.ApiService.Models;

namespace Multilinks.ApiService.Services
{
   public class NotificationService : INotificationService
   {
      private readonly ApiServiceDbContext _context;

      public NotificationService(ApiServiceDbContext context)
      {
         _context = context;
      }

      public async Task<NotificationEntity> GetNotificationByIdAsync(Guid id, CancellationToken ct)
      {
         var notification = await _context.Notifications
            .Where(r => r.Id == id)
            .Include(r => r.RecipientEndpoint).ThenInclude(r => r.Owner)
            .FirstOrDefaultAsync(ct);

         return notification;
      }

      public async Task<PagedResults<NotificationEntity>> GetNewNotificationsAsync(Guid endpointId,
         Guid ownerId,
         PagingOptions pagingOptions,
         CancellationToken ct)
      {
         IQueryable<NotificationEntity> query = _context.Notifications
            .Where(r => r.RecipientEndpoint.Owner.IdentityId == ownerId && r.RecipientEndpoint.EndpointId == endpointId && !r.Hidden)
            .Include(r => r.RecipientEndpoint).ThenInclude(r => r.Owner);

         var size = await query.CountAsync(ct);

         var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ToArrayAsync(ct);

         return new PagedResults<NotificationEntity>
         {
            Items = items,
            TotalSize = size
         };
      }

      public async Task<bool> CreateNotificationAsync(NotificationEntity notification, CancellationToken ct)
      {
         _context.Notifications.Add(notification);

         var created = await _context.SaveChangesAsync(ct);

         return created > 0 ? true : false;
      }

      public async Task<bool> UpdateHiddenStatusByIdAsync(Guid id, bool hidden, CancellationToken ct)
      {
         var notification = await _context.Notifications.FirstOrDefaultAsync(r => r.Id == id, ct);

         if(notification == null)
            return false;

         notification.Hidden = hidden;

         var updated = await _context.SaveChangesAsync();

         if(updated < 1)
            return false;

         return true;
      }
   }
}
