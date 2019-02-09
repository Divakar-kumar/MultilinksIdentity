using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Services
{
   public class LinkService : ILinkService
   {
      private readonly ApiServiceDbContext _context;

      public LinkService(ApiServiceDbContext context)
      {
         _context = context;
      }

      public async Task<bool> CheckEndpointOwnedByUserAsync(Guid creatorId, Guid endpointId, CancellationToken ct)
      {
         var entity = await _context.Endpoints.SingleOrDefaultAsync(
            r => (r.CreatorId == creatorId && r.EndpointId == endpointId),
            ct);

         if(entity == null) return false;

         return true;
      }
   }
}
