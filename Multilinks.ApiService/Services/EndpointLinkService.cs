using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Multilinks.ApiService.Entities;
using Multilinks.ApiService.Models;
using System;
using System.Linq;
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

      public async Task<EndpointLinkEntity> GetLinkByEndpointsIdAsync(Guid sourceEndpointId,
         Guid associatedEndpointId,
         CancellationToken ct)
      {
         var link = await _context.Links
            .Where(r => (r.SourceEndpoint.EndpointId == sourceEndpointId && r.AssociatedEndpoint.EndpointId == associatedEndpointId))
            .Include(r => r.AssociatedEndpoint).ThenInclude(r => r.Owner)
            .Include(r => r.AssociatedEndpoint).ThenInclude(r => r.HubConnection)
            .FirstOrDefaultAsync(ct);

         return link;
      }

      public async Task<EndpointLinkEntity> GetLinkByIdAsync(Guid linkId, CancellationToken ct)
      {
         var link = await _context.Links
            .Where(r => (r.LinkId == linkId))
            .Include(r => r.SourceEndpoint).ThenInclude(r => r.HubConnection)
            .Include(r => r.AssociatedEndpoint).ThenInclude(r => r.Owner)
            .Include(r => r.AssociatedEndpoint).ThenInclude(r => r.HubConnection)
            .FirstOrDefaultAsync(ct);

         return link;
      }

      public async Task<PagedResults<EndpointLinkEntity>> GetEndpointLinksPendingAsync(Guid endpointId,
         Guid ownerId,
         PagingOptions pagingOptions,
         CancellationToken ct)
      {
         IQueryable<EndpointLinkEntity> query = _context.Links
            .Where(r => r.AssociatedEndpoint.Owner.IdentityId == ownerId && r.AssociatedEndpoint.EndpointId == endpointId && !r.Confirmed)
            .Include(r => r.SourceEndpoint).ThenInclude(r => r.Owner);

         var size = await query.CountAsync(ct);

         var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ToArrayAsync(ct);

         return new PagedResults<EndpointLinkEntity>
         {
            Items = items,
            TotalSize = size
         };
      }

      public async Task<PagedResults<EndpointLinkEntity>> GetEndpointLinksConfirmedAsync(Guid sourceId,
         Guid ownerId,
         PagingOptions pagingOptions,
         CancellationToken ct)
      {
         IQueryable<EndpointLinkEntity> query = _context.Links
            .Where(r => r.SourceEndpoint.Owner.IdentityId == ownerId && r.SourceEndpoint.EndpointId == sourceId && r.Confirmed)
            .Include(r => r.AssociatedEndpoint).ThenInclude(r => r.Owner)
            .Include(r => r.AssociatedEndpoint).ThenInclude(r => r.HubConnection);

         var size = await query.CountAsync(ct);

         var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ToArrayAsync(ct);

         return new PagedResults<EndpointLinkEntity>
         {
            Items = items,
            TotalSize = size
         };
      }

      public async Task<EndpointLinkEntity> CreateLinkAsync(EndpointEntity sourceEndpoint,
         EndpointEntity associatedEndpoint,
         CancellationToken ct)
      {
         /* The assumption here is that we have already check that a link from sourceEndpoint to
          * associatedEndpoint doesn't exist so we can just go ahead and create a link. */
         var link = new EndpointLinkEntity
         {
            SourceEndpoint = sourceEndpoint,
            AssociatedEndpoint = associatedEndpoint,
            Confirmed = false
         };

         _context.Links.Add(link);

         var created = await _context.SaveChangesAsync(ct);

         if(created < 1)
            return null;

         link = await _context.Links
            .Where(r => (r.SourceEndpoint.EndpointId == sourceEndpoint.EndpointId && r.AssociatedEndpoint.EndpointId == associatedEndpoint.EndpointId))
            .Include(r => r.SourceEndpoint).ThenInclude(r => r.Owner)
            .Include(r => r.AssociatedEndpoint).ThenInclude(r => r.HubConnection)
            .FirstOrDefaultAsync(ct);

         return link;
      }

      public async Task<bool> UpdateLinkStatusByIdAsync(Guid linkId, bool confirmed, CancellationToken ct)
      {
         var link = await _context.Links.FirstOrDefaultAsync(r => r.LinkId == linkId, ct);

         if(link == null)
            return false;

         link.Confirmed = confirmed;

         var updated = await _context.SaveChangesAsync();

         if(updated < 1)
            return false;

         return true;
      }

      public async Task<bool> DeleteLinkByIdAsync(Guid linkId, Guid userId, CancellationToken ct)
      {
         /* Only owners of the endpoints can delete links. */
         var link = await _context.Links
            .Where(r => (r.LinkId == linkId && (r.SourceEndpoint.Owner.IdentityId == userId || r.AssociatedEndpoint.Owner.IdentityId == userId)))
            .FirstOrDefaultAsync(ct);

         if(link == null)
            return true;

         _context.Links.Remove(link);

         var deleted = await _context.SaveChangesAsync(ct);

         if(deleted < 1)
            return false;

         return true;
      }
   }
}
