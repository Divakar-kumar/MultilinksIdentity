using System;
using System.Threading;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Services
{
   public interface IHubConnectionService
   {
      Task<bool> ConnectHubConnectionReferenceAsync(Guid endpointId, Guid ownerId, string connectionId, CancellationToken ct);

      Task<bool> DisconnectHubConnectionReferenceAsync(string connectionId, CancellationToken ct);
   }
}
