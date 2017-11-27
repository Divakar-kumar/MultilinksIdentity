using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multilinks.ApiService.Models;
using Multilinks.ApiService.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Controllers
{
   [Route("api/[controller]")]
   [ApiVersion("1.0")]
   public class EndpointsController : Controller
   {
      private readonly IEndpointService _endpointService;

      public EndpointsController(IEndpointService endpointService)
      {
         _endpointService = endpointService;
      }

      [HttpGet(Name = nameof(GetEndpointsAsync))]
      public async Task<IActionResult> GetEndpointsAsync(CancellationToken ct)
      {
         var endpoints = await _endpointService.GetEndpointsAsync(ct);

         var collectionLink = Link.ToCollection(nameof(GetEndpointsAsync));
         var collection = new Collection<EndpointViewModel>
         {
            Self = collectionLink,
            Value = endpoints.ToArray()
         };

         return Ok(collection);
      }

      // GET api/endpoints/{endpointId}
      [HttpGet("{endpointId}", Name = nameof(GetEndpointByIdAsync))]
      public async Task<IActionResult> GetEndpointByIdAsync(Guid endpointId, CancellationToken ct)
      {
         var endpointViewModel = await _endpointService.GetEndpointAsync(endpointId, ct);
         if(endpointViewModel == null) return NotFound();

         return Ok(endpointViewModel);
      }
   }
}
