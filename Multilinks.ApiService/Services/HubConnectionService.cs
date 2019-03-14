using Microsoft.EntityFrameworkCore;
using Multilinks.ApiService.Entities;
using System;
using System.Linq;
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

      public async Task<bool> ConnectHubConnectionReferenceAsync(Guid endpointId, Guid ownerId, string connectionId, CancellationToken ct)
      {
         var endpoint = await _context.Endpoints
            .Where(r => r.EndpointId == endpointId && r.Owner.IdentityId == ownerId)
            .Include(r => r.HubConnection)
            .FirstOrDefaultAsync(ct);

         if(endpoint == null)
            return false;

         endpoint.HubConnection.ConnectionId = connectionId;
         endpoint.HubConnection.Connected = true;

         var updated = await _context.SaveChangesAsync(ct);
         if(updated < 1) return false;

         return true;
      }

      public async Task<bool> DisconnectHubConnectionReferenceAsync(string connectionId, CancellationToken ct)
      {
         var hubConnection = await _context.HubConnections.FirstOrDefaultAsync(r => r.ConnectionId == connectionId, ct);

         if(hubConnection != null)
         {
            hubConnection.ConnectionId = "";
            hubConnection.Connected = false;

            var updated = await _context.SaveChangesAsync(ct);
            if(updated < 1) return false;
         }

         return true;
      }
   }
}
