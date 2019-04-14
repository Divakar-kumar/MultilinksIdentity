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

      Task<PagedResults<EndpointLinkEntity>> GetEndpointLinksPendingAsync(Guid endpointId,
         Guid ownerId,
         PagingOptions pagingOptions,
         CancellationToken ct);

      Task<PagedResults<EndpointLinkEntity>> GetEndpointLinksConfirmedAsync(Guid sourceId,
         Guid ownerId,
         PagingOptions pagingOptions,
         CancellationToken ct);

      Task<EndpointLinkEntity> CreateLinkAsync(EndpointEntity sourceEndpoint,
         EndpointEntity associatedEndpoint,
         CancellationToken ct);

      Task<bool> UpdateLinkStatusByIdAsync(Guid linkId, bool confirmed, CancellationToken ct);

      Task<bool> DeleteLinkByIdAsync(Guid linkId, Guid userId, CancellationToken ct);
   }
}
