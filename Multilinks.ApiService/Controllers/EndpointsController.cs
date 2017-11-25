using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multilinks.ApiService.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Controllers
{
   [Route("api/[controller]")]
   [ApiVersion("1.0")]
   [Authorize]
   public class EndpointsController : Controller
   {
      private readonly IEndpointService _endpointService;

      public EndpointsController(IEndpointService endpointService)
      {
         _endpointService = endpointService;
      }

      [HttpGet(Name = nameof(GetEndpoints))]
      public IActionResult GetEndpoints()
      {
         throw new NotImplementedException();
      }

      // GET api/endpoints/{endpointId}
      [HttpGet("{endpointId}", Name = nameof(GetEndpointByIdAsync))]
      [AllowAnonymous]
      public async Task<IActionResult> GetEndpointByIdAsync(Guid endpointId, CancellationToken ct)
      {
         var endpointViewModel = await _endpointService.GetEndpointAsync(endpointId, ct);
         if(endpointViewModel == null) return NotFound();

         return Ok(endpointViewModel);
      }
   }
}
