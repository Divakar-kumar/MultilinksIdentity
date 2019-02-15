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

      public async Task<EndpointLinkViewModel> GetEndpointLinkByIdAsync(Guid endpointA, Guid endpointB, CancellationToken ct)
      {
         var entity = await _context.Links.SingleOrDefaultAsync(
            r => (r.FirstEndpointId == endpointA && r.SecondEndpointId == endpointB) || (r.FirstEndpointId == endpointB && r.SecondEndpointId == endpointA),
            ct);

         if(entity == null) return null;

         return Mapper.Map<EndpointLinkViewModel>(entity);
      }

      public async Task<Guid> CreateEndpointLinkAsync(Guid endpointA, Guid endpointB, CancellationToken ct)
      {
         var linkId = Guid.NewGuid();

         var newEndpointLink = new EndpointLinkEntity
         {
            LinkId = linkId,
            FirstEndpointId = endpointA,
            SecondEndpointId = endpointB,
            Status = "pending"
         };

         _context.Links.Add(newEndpointLink);

         var created = await _context.SaveChangesAsync(ct);

         if(created < 1) throw new InvalidOperationException("Could not create new link.");

         return newEndpointLink.LinkId;
      }
   }
}
