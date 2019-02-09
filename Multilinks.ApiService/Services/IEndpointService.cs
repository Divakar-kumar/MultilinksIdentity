using Multilinks.ApiService.Entities;
using Multilinks.ApiService.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Services
{
   public interface IEndpointService
   {
      Task<EndpointViewModel> GetEndpointByIdAsync(Guid endpointId, CancellationToken ct);

      Task<EndpointViewModel> GetOwnEndpointByNameAsync(string name, CancellationToken ct);

      Task<bool> CheckEndpointByNameOwnedByUserExistsAsync(Guid creatorId, string endpointName, CancellationToken ct);

      Task<PagedResults<EndpointViewModel>> GetEndpointsAsync(PagingOptions pagingOptions,
                                                              SortOptions<EndpointViewModel, EndpointEntity> sortOptions,
                                                              SearchOptions<EndpointViewModel, EndpointEntity> searchOptions,
                                                              CancellationToken ct);

      Task<PagedResults<EndpointViewModel>> GetEndpointsByCreatorIdAsync(Guid creatorId,
                                                                         PagingOptions pagingOptions,
                                                                         SortOptions<EndpointViewModel, EndpointEntity> sortOptions,
                                                                         SearchOptions<EndpointViewModel, EndpointEntity> searchOptions,
                                                                         CancellationToken ct);

      Task<Guid> CreateEndpointAsync(Guid creatorId,
                                     string name,
                                     string description,
                                     CancellationToken ct);

      Task<bool> DeleteEndpointByIdAsync(Guid endpointId, CancellationToken ct);

      Task<bool> UpdateEndpointByIdAsync(Guid endpointId,
                                         string description,
                                         CancellationToken ct);

      Task<bool> CheckEndpointIsCreatedByUser(Guid endpointId, Guid creatorId, CancellationToken ct);


   }
}
