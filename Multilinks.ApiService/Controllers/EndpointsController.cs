using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
      private readonly PagingOptions _defaultPagingOptions;

      public EndpointsController(IEndpointService endpointService, IOptions<PagingOptions> defaultPagingOptions)
      {
         _endpointService = endpointService;
         _defaultPagingOptions = defaultPagingOptions.Value;
      }

      [HttpGet(Name = nameof(GetEndpointsAsync))]
      public async Task<IActionResult> GetEndpointsAsync(
         [FromQuery] PagingOptions pagingOptions,
         [FromQuery] SortOptions<EndpointViewModel, EndpointEntity> sortOptions,
         [FromQuery] SearchOptions<EndpointViewModel, EndpointEntity> searchOptions,
         CancellationToken ct)
      {
         if(!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

         pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
         pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

         var endpoints = await _endpointService.GetEndpointsAsync(pagingOptions, sortOptions, searchOptions, ct);

         var collection = PagedCollection<EndpointViewModel>.Create(
            Link.ToCollection(nameof(GetEndpointsAsync)),
            endpoints.Items.ToArray(),
            endpoints.TotalSize,
            pagingOptions
            );

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
