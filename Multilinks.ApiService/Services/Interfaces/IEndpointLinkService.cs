using Multilinks.ApiService.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Services
{
   public interface IEndpointLinkService
   {
      Task<EndpointLinkViewModel> GetLinkByEndpointsIdAsync(Guid endpointA, Guid endpointB, CancellationToken ct);

      Task<EndpointLinkViewModel> GetLinkByIdAsync(Guid linkId, CancellationToken ct);

      Task<EndpointLinkViewModel> CreateEndpointLinkAsync(Guid endpointA, Guid endpointB, CancellationToken ct);
   }
}
