using Multilinks.ApiService.Entities;
using Multilinks.ApiService.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Services
{
   public interface IEndpointLinkService
   {
      Task<EndpointLinkViewModel> GetLinkByEndpointsIdAsync(Guid sourceEndpointId,
         Guid associatedEndpointId,
         CancellationToken ct);

      Task<EndpointLinkViewModel> GetLinkByIdAsync(Guid linkId, CancellationToken ct);

      Task<EndpointLinkViewModel> CreateEndpointLinkAsync(EndpointEntity sourceEndpoint,
         EndpointEntity associatedEndpoint,
         CancellationToken ct);
   }
}
