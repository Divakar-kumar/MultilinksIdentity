using System;
using System.Threading;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Services
{
   public interface ILinkService
   {
      Task<bool> CheckEndpointOwnedByUserAsync(Guid creatorId, Guid endpointId, CancellationToken ct);
   }
}
