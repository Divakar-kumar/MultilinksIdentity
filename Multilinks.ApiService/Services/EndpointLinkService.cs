using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Multilinks.ApiService.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Services
{
   public class EndpointLinkService : IEndpointLinkService
   {
      private readonly ApiServiceDbContext _context;

      public EndpointLinkService(ApiServiceDbContext context)
      {
         _context = context;
      }

      public async Task<EndpointLinkViewModel> GetEndpointLinkByIdAsync(Guid endpointA, Guid endpointB, CancellationToken ct)
      {
         var entity = await _context.Links.SingleOrDefaultAsync(
            r => (r.FirstEndpointId == endpointA && r.SecondEndpointId == endpointB) || (r.FirstEndpointId == endpointB && r.SecondEndpointId == endpointA),
            ct);

         if(entity == null) return null;

         return Mapper.Map<EndpointLinkViewModel>(entity);
      }
   }
}
