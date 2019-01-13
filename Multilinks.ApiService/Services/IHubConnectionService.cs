using System;
using System.Threading;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Services
{
   public interface IHubConnectionService
   {
      Task<bool> CreateHubConnectionReferenceAsync(Guid endpointId, CancellationToken ct);

      Task<bool> DeleteHubConnectionReferenceAsync(string connectionId, CancellationToken ct);

      Task<bool> UpdateHubConnectionReferenceAsync(Guid endpointId, string connectionId, CancellationToken ct);
   }
}
