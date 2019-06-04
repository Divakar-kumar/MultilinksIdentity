using System;
using System.Threading;
using System.Threading.Tasks;
using Multilinks.ApiService.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Multilinks.ApiService.Entities;

namespace Multilinks.ApiService.Services
{
   public class EndpointService : IEndpointService
   {
      private readonly ApiServiceDbContext _context;

      public EndpointService(ApiServiceDbContext context)
      {
         _context = context;
      }

      public async Task<EndpointEntity> GetEndpointByIdAsync(Guid endpointId, CancellationToken ct)
      {
         var endpoint = await _context.Endpoints
            .Where(r => r.EndpointId == endpointId)
            .Include(r => r.Owner)
            .Include(r => r.Client)
            .FirstOrDefaultAsync(ct);

         return endpoint;
      }

      public async Task<EndpointEntity> GetEndpointByNameAsync(string name, Guid ownerId, CancellationToken ct)
      {
         var endpoint = await _context.Endpoints
            .Where(r => r.Name == name && r.Owner.IdentityId == ownerId)
            .Include(r => r.Owner)
            .Include(r => r.Client)
            .FirstOrDefaultAsync(ct);

         return endpoint;
      }

      public async Task<EndpointEntity> CreateEndpointAsync(string name,
                                                            EndpointClientEntity client,
                                                            EndpointOwnerEntity owner,
                                                            HubConnectionEntity hubConnection,
                                                            CancellationToken ct)
      {
         var endpoint = await _context.Endpoints.FirstOrDefaultAsync(
            r => (r.Owner.IdentityId == owner.IdentityId && r.Name == name),
            ct);

         /* Endpoint already exists so return null to indicate we failed to create new endpoint. */
         if(endpoint != null)
            return null;

         var existingClient = await _context.Clients.FirstOrDefaultAsync(
            r => (r.ClientId == client.ClientId && r.ClientType == client.ClientType),
            ct);

         /* Ensure client exist */
         if(existingClient == null)
            _context.Clients.Add(client);

         var existingOwner = await _context.Owners.FirstOrDefaultAsync(
            r => (r.IdentityId == owner.IdentityId && r.OwnerName == owner.OwnerName),
            ct);

         /* Ensure owner exist */
         if(existingOwner == null)
            _context.Owners.Add(owner);

         if(existingClient == null || existingOwner == null)
         {
            var createdNavigationPropertiesResult = await _context.SaveChangesAsync(ct);

            if(createdNavigationPropertiesResult < 1)
               return null;

            if(existingClient == null)
            {
               existingClient = await _context.Clients.FirstOrDefaultAsync(
                  r => (r.ClientId == client.ClientId && r.ClientType == client.ClientType),
                  ct);
            }

            if(existingOwner == null)
            {
               existingOwner = await _context.Owners.FirstOrDefaultAsync(
                  r => (r.IdentityId == owner.IdentityId && r.OwnerName == owner.OwnerName),
                  ct);
            }
         }

         endpoint = new EndpointEntity
         {
            Name = name,
            Description = "No description.",
            Client = existingClient,
            Owner = existingOwner,
            HubConnection = hubConnection
         };

         if(endpoint.Client.ClientId == "WebConsole")
         {
            endpoint.Description = "This web console is the default interface for your Multilinks account.";
         }

         _context.Endpoints.Add(endpoint);

         var created = await _context.SaveChangesAsync(ct);

         if(created < 1)
            return null;

         /* Everything should be up to date now, so let's try again and return the endpoint. */
         endpoint = await _context.Endpoints
            .Where(r => r.Name == name && r.Owner.IdentityId == existingOwner.IdentityId)
            .Include(r => r.Owner)
            .Include(r => r.Client)
            .FirstOrDefaultAsync();

         return endpoint;
      }

      public async Task<PagedResults<EndpointEntity>> GetEndpointsByOwnerIdAsync(Guid ownerId,
                                                                                 PagingOptions pagingOptions,
                                                                                 SortOptions<EndpointViewModel, EndpointEntity> sortOptions,
                                                                                 SearchOptions<EndpointViewModel, EndpointEntity> searchOptions,
                                                                                 CancellationToken ct)
      {
         IQueryable<EndpointEntity> query = _context.Endpoints
            .Where(r => r.Owner.IdentityId == ownerId)
            .Include(r => r.Owner)
            .Include(r => r.Client);

         query = searchOptions.Apply(query);
         query = sortOptions.Apply(query);

         var size = await query.CountAsync(ct);

         var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ToArrayAsync(ct);

         return new PagedResults<EndpointEntity>
         {
            Items = items,
            TotalSize = size
         };
      }

      public async Task<bool> DeleteEndpointByIdAsync(Guid endpointId, CancellationToken ct)
      {
         var endpoint = await _context.Endpoints.FirstOrDefaultAsync(r => r.EndpointId == endpointId, ct);

         /* Still returns true if specified endpoint doesn't exist. */
         if(endpoint == null) return true;

         _context.Endpoints.Remove(endpoint);

         var deleted = await _context.SaveChangesAsync(ct);

         /* We tried to delete it but failed, so return false. */
         if(deleted < 1) return false;

         return true;
      }

      public async Task<bool> UpdateEndpointByIdAsync(Guid endpointId,
                                                      string description,
                                                      CancellationToken ct)
      {
         var endpoint = await _context.Endpoints.FirstOrDefaultAsync(r => r.EndpointId == endpointId, ct);

         if(endpoint == null)
            return false;

         endpoint.Description = description;

         var updated = await _context.SaveChangesAsync();

         if(updated < 1) return false;

         return true;
      }
   }
}
