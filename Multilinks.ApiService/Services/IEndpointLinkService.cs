using Multilinks.ApiService.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Services
{
   public interface IEndpointLinkService
   {
      Task<EndpointLinkViewModel> GetEndpointLinkByIdAsync(Guid endpointA, Guid endpointB, CancellationToken ct);
   }
}
