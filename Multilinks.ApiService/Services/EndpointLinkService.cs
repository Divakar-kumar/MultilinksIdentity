using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Multilinks.ApiService.Entities;
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

      public async Task<EndpointLinkViewModel> GetLinkByEndpointsIdAsync(Guid endpointA, Guid endpointB, CancellationToken ct)
      {
         var entity = await _context.Links.SingleOrDefaultAsync(
            r => (r.SourceEndpointId == endpointA && r.AssociatedEndpointId == endpointB),
            ct);

         if(entity == null) return null;

         return Mapper.Map<EndpointLinkViewModel>(entity);
      }

      public async Task<EndpointLinkViewModel> GetLinkByIdAsync(Guid linkId, CancellationToken ct)
      {
         var entity = await _context.Links.SingleOrDefaultAsync(r => r.LinkId == linkId, ct);

         if(entity == null) return null;

         return Mapper.Map<EndpointLinkViewModel>(entity);
      }

      public async Task<EndpointLinkViewModel> CreateEndpointLinkAsync(Guid endpointA, Guid endpointB, CancellationToken ct)
      {
         var linkId = Guid.NewGuid();

         var newEndpointLink = new EndpointLinkEntity
         {
            LinkId = linkId,
            SourceEndpointId = endpointA,
            AssociatedEndpointId = endpointB,
            Status = "pending"
         };

         _context.Links.Add(newEndpointLink);

         var created = await _context.SaveChangesAsync(ct);

         if(created < 1) throw new InvalidOperationException("Could not create new link.");

         return Mapper.Map<EndpointLinkViewModel>(newEndpointLink);
      }

   }
}
