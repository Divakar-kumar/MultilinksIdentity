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

      Task<IEnumerable<EndpointViewModel>> GetEndpointsAsync(CancellationToken ct);
   }
}
