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

      public async Task<EndpointLinkViewModel> GetLinkByEndpointsIdAsync(Guid sourceEndpointId,
         Guid associatedEndpointId,
         CancellationToken ct)
      {
         var entity = await _context.Links.FirstOrDefaultAsync(
            r => (r.SourceEndpoint.EndpointId == sourceEndpointId && r.AssociatedEndpoint.EndpointId == associatedEndpointId),
            ct);

         if(entity == null) return null;

         return Mapper.Map<EndpointLinkViewModel>(entity);
      }

      public async Task<EndpointLinkViewModel> GetLinkByIdAsync(Guid linkId, CancellationToken ct)
      {
         var entity = await _context.Links.FirstOrDefaultAsync(r => r.LinkId == linkId, ct);

         if(entity == null) return null;

         return Mapper.Map<EndpointLinkViewModel>(entity);
      }

      public async Task<EndpointLinkViewModel> CreateEndpointLinkAsync(EndpointEntity sourceEndpoint,
         EndpointEntity associatedEndpoint,
         CancellationToken ct)
      {
         var endpointLink = new EndpointLinkEntity
         {
            SourceEndpoint = sourceEndpoint,
            AssociatedEndpoint = associatedEndpoint,
            Status = "pending"
         };

         _context.Links.Add(endpointLink);

         var created = await _context.SaveChangesAsync(ct);

         if(created < 1) throw new InvalidOperationException("Could not create new link.");

         endpointLink = await _context.Links.FirstOrDefaultAsync(
            r => (r.SourceEndpoint.EndpointId == sourceEndpoint.EndpointId && r.AssociatedEndpoint.EndpointId == associatedEndpoint.EndpointId),
            ct);

         return Mapper.Map<EndpointLinkViewModel>(endpointLink);
      }

   }
}
