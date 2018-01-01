using System;
using System.Threading;
using System.Threading.Tasks;
using Multilinks.ApiService.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Linq;
using Multilinks.DataService.Entities;
using Multilinks.DataService;

namespace Multilinks.ApiService.Services
{
   public class EndpointService : IEndpointService
   {
      private readonly ApplicationDbContext _context;

      public EndpointService(ApplicationDbContext context)
      {
         _context = context;
      }

      public async Task<EndpointViewModel> GetEndpointByIdAsync(Guid id, CancellationToken ct)
      {
         var entity = await _context.Endpoints.SingleOrDefaultAsync(r => r.EndpointId == id, ct);
         if(entity == null) return null;

         return Mapper.Map<EndpointViewModel>(entity);
      }

      public async Task<bool> CheckEndpointExistsAsync(Guid creatorId, string name, CancellationToken ct)
      {
         var entity = await _context.Endpoints.SingleOrDefaultAsync(
            r => (r.CreatorId == creatorId  && r.Name == name),
            ct);
         if(entity == null) return false;

         return true;
      }

      public async Task<bool> CheckGatewayExistsAsync(Guid serviceAreaId, CancellationToken ct)
      {
         var entity = await _context.Endpoints.SingleOrDefaultAsync(r => r.ServiceAreaId == serviceAreaId, ct);
         if(entity == null) return false;

         return true;
      }

      public async Task<PagedResults<EndpointViewModel>> GetEndpointsAsync(
         PagingOptions pagingOptions,
         SortOptions<EndpointViewModel, EndpointEntity> sortOptions,
         SearchOptions<EndpointViewModel, EndpointEntity> searchOptions,
         CancellationToken ct)
      {
         IQueryable<EndpointEntity> query = _context.Endpoints;
         query = searchOptions.Apply(query);
         query = sortOptions.Apply(query);

         var size = await query.CountAsync(ct);

         var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<EndpointViewModel>()
                .ToArrayAsync(ct);

         return new PagedResults<EndpointViewModel>
         {
            Items = items,
            TotalSize = size
         };
      }

      public async Task<Guid> CreateEndpointAsync(Guid serviceAreaId,
                                     Guid creatorId,
                                     bool isCloudConnected,
                                     bool isGateway,
                                     EndpointEntity.CommsDirectionCapabilities commCapability,
                                     string name,
                                     string description,
                                     CancellationToken ct)
      {
         var endpointId = Guid.NewGuid();

         if(isGateway) serviceAreaId = endpointId;

         var newEndpoint = new EndpointEntity
         {
            EndpointId = endpointId,
            ServiceAreaId = serviceAreaId,
            CreatorId = creatorId,
            IsCloudConnected = isCloudConnected,
            IsGateway = isGateway,
            DirectionCapability = commCapability,
            Name = name,
            Description = description
         };

         _context.Endpoints.Add(newEndpoint);

         var created = await _context.SaveChangesAsync(ct);

         if(created < 1) throw new InvalidOperationException("Could not create new endpoint.");

         return newEndpoint.EndpointId;
      }

      public async Task DeleteEndpointByIdAsync(Guid endpointId, CancellationToken ct)
      {
         var endpoint = await _context.Endpoints.SingleOrDefaultAsync(ep => ep.EndpointId == endpointId, ct);

         if(endpoint == null) return;

         _context.Endpoints.Remove(endpoint);
         var deleted = await _context.SaveChangesAsync(ct);
         if(deleted < 1) throw new InvalidOperationException("Could not delete endpoint.");
      }
   }
}
