using Multilinks.ApiService.Entities;
using Multilinks.ApiService.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Services
{
   public interface IEndpointService
   {
      Task<EndpointEntity> GetEndpointByIdAsync(Guid endpointId, CancellationToken ct);

      Task<EndpointEntity> GetEndpointByNameAsync(string name, Guid ownerId, CancellationToken ct);

      Task<EndpointEntity> CreateEndpointAsync(string name,
                                               EndpointClientEntity client,
                                               EndpointOwnerEntity owner,
                                               HubConnectionEntity hubConnection,
                                               CancellationToken ct);

      Task<PagedResults<EndpointEntity>> GetEndpointsByOwnerIdAsync(Guid ownerId,
                                                                    PagingOptions pagingOptions,
                                                                    SortOptions<EndpointViewModel, EndpointEntity> sortOptions,
                                                                    SearchOptions<EndpointViewModel, EndpointEntity> searchOptions,
                                                                    CancellationToken ct);

      Task<bool> DeleteEndpointByIdAsync(Guid endpointId, CancellationToken ct);

      Task<bool> UpdateEndpointByIdAsync(Guid endpointId,
                                         string description,
                                         CancellationToken ct);
   }
}
