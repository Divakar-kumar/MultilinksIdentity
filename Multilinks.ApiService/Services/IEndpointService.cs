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

      Task<bool> CheckGatewayExistsAsync(Guid serviceAreaId, CancellationToken ct);

      Task<PagedResults<EndpointViewModel>> GetEndpointsAsync(
         PagingOptions pagingOptions,
         SortOptions<EndpointViewModel, EndpointEntity> sortOptions,
         SearchOptions<EndpointViewModel, EndpointEntity> searchOptions,
         CancellationToken ct);

      Task<Guid> CreateEndpointAsync(Guid serviceAreaId,
                                     Guid creatorId,
                                     bool isCloudConnected,
                                     bool isGateway,
                                     EndpointEntity.CommsDirectionCapabilities commCapability,
                                     string name,
                                     string description,
                                     CancellationToken ct);
   }
}
