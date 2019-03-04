using System;
using System.Threading;
using System.Threading.Tasks;
using Multilinks.ApiService.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Linq;
using Multilinks.ApiService.Entities;

namespace Multilinks.ApiService.Services
{
   public class EndpointService : IEndpointService
   {
      private readonly ApiServiceDbContext _context;
      private readonly IUserInfoService _userInfoService;

      public EndpointService(ApiServiceDbContext context,
         IUserInfoService userInfoService)
      {
         _context = context;
         _userInfoService = userInfoService;
      }

      public async Task<EndpointViewModel> GetEndpointByIdAsync(Guid id, CancellationToken ct)
      {
         var entity = await _context.Endpoints.FirstOrDefaultAsync(r => r.EndpointId == id, ct);
         if(entity == null) return null;

         return Mapper.Map<EndpointViewModel>(entity);
      }

      public async Task<EndpointViewModel> GetOwnEndpointByNameAsync(string endpointName, CancellationToken ct)
      {
         var endpoint = await _context.Endpoints.FirstOrDefaultAsync(
            r => (r.Owner.IdentityId == _userInfoService.UserId && r.Name == endpointName),
            ct);

         if(endpoint == null)
         {
            var client = await _context.Clients.FirstOrDefaultAsync(
               r => (r.ClientId == _userInfoService.ClientId && r.ClientType == _userInfoService.ClientType),
               ct);

            if(client == null)
            {
               client = new EndpointClientEntity
               {
                  ClientId = _userInfoService.ClientId,
                  ClientType = _userInfoService.ClientType
               };
            }

            var owner = await _context.Owners.FirstOrDefaultAsync(
               r => (r.IdentityId == _userInfoService.UserId && r.OwnerName == _userInfoService.Name),
               ct);

            if(owner == null)
            {
               owner = new EndpointOwnerEntity
               {
                  IdentityId = _userInfoService.UserId,
                  OwnerName = _userInfoService.Name
               };
            }

            endpoint = new EndpointEntity
            {
               Name = endpointName,
               Description = "No description.",
               Client = client,
               Owner = owner
            };

            _context.Endpoints.Add(endpoint);

            var created = await _context.SaveChangesAsync(ct);

            if(created < 1) throw new InvalidOperationException("Could not create new endpoint.");
         }

         return Mapper.Map<EndpointViewModel>(endpoint);
      }

      public async Task<bool> CheckEndpointByNameCreatedBySpecifiedUserExistsAsync(Guid creatorId, string endpointName, CancellationToken ct)
      {
         var entity = await _context.Endpoints.FirstOrDefaultAsync(
            r => (r.Owner.IdentityId == creatorId && r.Name == endpointName),
            ct);

         if(entity == null) return false;

         return true;
      }

      public async Task<PagedResults<EndpointViewModel>> GetEndpointsAsync(PagingOptions pagingOptions,
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

      public async Task<PagedResults<EndpointViewModel>> GetEndpointsByCreatorIdAsync(Guid creatorId,
                                                                                      PagingOptions pagingOptions,
                                                                                      SortOptions<EndpointViewModel, EndpointEntity> sortOptions,
                                                                                      SearchOptions<EndpointViewModel, EndpointEntity> searchOptions,
                                                                                      CancellationToken ct)
      {
         IQueryable<EndpointEntity> query = _context.Endpoints;
         query = query.Where(ep => ep.Owner.IdentityId == creatorId);
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

      public async Task<Guid> CreateEndpointAsync(string name,
                                                  string description,
                                                  CancellationToken ct)
      {
         var endpointId = Guid.NewGuid();

         var newEndpoint = new EndpointEntity
         {
            Name = name,
            Description = description,

            // TODO: Use existing if exist
            Client = new EndpointClientEntity
            {
               ClientId = _userInfoService.ClientId,
               ClientType = _userInfoService.ClientType
            },

            // TODO: Use existing if exist
            Owner = new EndpointOwnerEntity
            {
               IdentityId = _userInfoService.UserId,
               OwnerName = _userInfoService.Name
            }



         };

         _context.Endpoints.Add(newEndpoint);

         var created = await _context.SaveChangesAsync(ct);

         if(created < 1) throw new InvalidOperationException("Could not create new endpoint.");

         return newEndpoint.EndpointId;
      }

      public async Task<bool> DeleteEndpointByIdAsync(Guid endpointId, CancellationToken ct)
      {
         var endpoint = await _context.Endpoints.FirstOrDefaultAsync(ep => ep.EndpointId == endpointId, ct);

         if(endpoint == null) return false;

         _context.Endpoints.Remove(endpoint);

         var deleted = await _context.SaveChangesAsync(ct);

         if(deleted < 1) return false;

         return true;
      }

      public async Task<bool> UpdateEndpointByIdAsync(Guid endpointId,
                                                      string description,
                                                      CancellationToken ct)
      {
         var entity = await _context.Endpoints.FirstOrDefaultAsync(r => r.EndpointId == endpointId, ct);

         if(entity == null) return false;

         entity.Description = description;

         var updated = await _context.SaveChangesAsync();

         if(updated < 1) return false;

         return true;
      }
   }
}
