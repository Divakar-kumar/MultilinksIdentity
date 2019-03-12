using Multilinks.ApiService.Entities;
using Multilinks.ApiService.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Services
{
   public interface IEndpointLinkService
   {
      Task<EndpointLinkEntity> GetLinkByEndpointsIdAsync(Guid sourceEndpointId,
         Guid associatedEndpointId,
         CancellationToken ct);

      Task<EndpointLinkEntity> GetLinkByIdAsync(Guid linkId, CancellationToken ct);

      Task<EndpointLinkEntity> CreateEndpointLinkAsync(EndpointEntity sourceEndpoint,
         EndpointEntity associatedEndpoint,
         CancellationToken ct);
   }
}
