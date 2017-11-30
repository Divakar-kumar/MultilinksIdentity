using Multilinks.ApiService.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Services
{
   public interface IEndpointService
   {
      Task<EndpointViewModel> GetEndpointAsync(Guid endpointId, CancellationToken ct);

      Task<PagedResults<EndpointViewModel>> GetEndpointsAsync(
         PagingOptions pagingOptions,
         SortOptions<EndpointViewModel, EndpointEntity> sortOptions,
         SearchOptions<EndpointViewModel, EndpointEntity> searchOptions,
         CancellationToken ct);
   }
}
