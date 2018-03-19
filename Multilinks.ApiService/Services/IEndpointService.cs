using Multilinks.ApiService.Models;
using Multilinks.DataService.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Services
{
   public interface IEndpointService
   {
      Task<EndpointViewModel> GetEndpointByIdAsync(Guid endpointId, CancellationToken ct);

      Task<bool> CheckEndpointExistsAsync(Guid creatorId, string name, CancellationToken ct);

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

      Task<EndpointViewModel> ReplaceEndpointByIdAsync(Guid endpointId,
                                                       Guid creatorId,
                                                       string name,
                                                       string description,
                                                       CancellationToken ct);
   }
}
