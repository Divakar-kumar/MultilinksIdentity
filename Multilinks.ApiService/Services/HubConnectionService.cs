using Microsoft.EntityFrameworkCore;
using Multilinks.ApiService.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Services
{
   public class HubConnectionService : IHubConnectionService
   {
      private readonly ApiServiceDbContext _context;
      private readonly IUserInfoService _userInfoService;

      public HubConnectionService(ApiServiceDbContext context, IUserInfoService userInfoService)
      {
         _context = context;
         _userInfoService = userInfoService;
      }

      public async Task<bool> CreateHubConnectionReferenceAsync(Guid endpointId, CancellationToken ct)
      {
         var entity = await _context.HubConnections.SingleOrDefaultAsync(
            r => (r.EndpointId == endpointId),
            ct);

         if(entity == null)
         {
            entity = new HubConnectionEntity
            {
               EndpointId = endpointId
            };

            _context.HubConnections.Add(entity);

            var created = await _context.SaveChangesAsync(ct);
            if(created < 1) return false;
         }

         return true;
      }

      public async Task<bool> DeleteHubConnectionReferenceAsync(string connectionId, CancellationToken ct)
      {
         var entity = await _context.HubConnections.SingleOrDefaultAsync(r => r.ConnectionId == connectionId, ct);

         if(entity != null)
         {
            _context.HubConnections.Remove(entity);
            var deleted = await _context.SaveChangesAsync(ct);
            if(deleted < 1) return false;
         }

         return true;
      }

      public async Task<bool> UpdateHubConnectionReferenceAsync(Guid endpointId, string connectionId, CancellationToken ct)
      {
         var entity = await _context.HubConnections.SingleOrDefaultAsync(r => r.EndpointId == endpointId, ct);

         if(entity == null) return false;

         entity.ConnectionId = connectionId;
         var updated = await _context.SaveChangesAsync();
         if(updated < 1) return false;

         return true;
      }
   }
}
