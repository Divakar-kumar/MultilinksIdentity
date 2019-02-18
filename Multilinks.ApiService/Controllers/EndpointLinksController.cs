using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multilinks.ApiService.Infrastructure;
using Multilinks.ApiService.Models;
using Multilinks.ApiService.Services;

namespace Multilinks.ApiService.Controllers
{
   [Route("api/[controller]")]
   [ApiVersion("1.0")]
   [Authorize]
   public class EndpointLinksController : Controller
   {
      private readonly IUserInfoService _userInfoService;
      private readonly IEndpointLinkService _linkService;
      private readonly IEndpointService _endpointService;

      public EndpointLinksController(IUserInfoService userInfoService,
         IEndpointLinkService linkService,
         IEndpointService endpointService)
      {
         _userInfoService = userInfoService;
         _linkService = linkService;
         _endpointService = endpointService;
      }

      // GET api/endpointlinks/id/{linkId}
      [HttpGet("id/{linkId}", Name = nameof(GetEndpointLinkByIdAsync))]
      [ResponseCache(CacheProfileName = "Resource")]
      [Etag]
      public async Task<IActionResult> GetEndpointLinkByIdAsync(Guid linkId, CancellationToken ct)
      {
         // TODO: Need more validation to ensure user has access to this link
         var endpointLinkViewModel = await _linkService.GetLinkByIdAsync(linkId, ct);

         if(endpointLinkViewModel == null)
         {
            return NotFound();
         }

         if(!Request.GetEtagHandler().NoneMatch(endpointLinkViewModel))
         {
            return StatusCode(304, endpointLinkViewModel);
         }

         return Ok(endpointLinkViewModel);
      }

      // POST api/endpointlinks
      [HttpPost(Name = nameof(CreateLinkAsync))]
      [ResponseCache(CacheProfileName = "Resource")]
      public async Task<IActionResult> CreateLinkAsync([FromBody] NewEndpointLinkForm newLink,
                                                       CancellationToken ct)
      {
         if(!ModelState.IsValid)
         {
            return BadRequest(new ApiError(ModelState));
         }

         var sourceId = Guid.Parse(newLink.Source);
         var destinationId = Guid.Parse(newLink.Destination);

         if(sourceId == destinationId)
         {
            return BadRequest(new ApiError("A device cannot create a link to itself."));
         }

         var endpointLink = await _linkService.GetLinkByEndpointsIdAsync(sourceId, destinationId, ct);

         if(endpointLink != null)
         {
            return BadRequest(new ApiError("Link between devices already exists."));
         }

         var sourceEndpoint = await _endpointService.GetEndpointByIdAsync(sourceId, ct);

         if(sourceEndpoint == null)
         {
            return BadRequest(new ApiError("Requesting device is invalid."));
         }

         if(sourceEndpoint.CreatorId != _userInfoService.UserId)
         {
            return BadRequest(new ApiError("Cannot create a link from a device not created by current user."));
         }

         var destinationEndpoint = await _endpointService.GetEndpointByIdAsync(destinationId, ct);

         if(destinationEndpoint == null)
         {
            return BadRequest(new ApiError("Requested device is invalid."));
         }

         var endpointLinkViewModel = await _linkService.CreateEndpointLinkAsync(sourceId, destinationId, ct);

         if(endpointLinkViewModel == null)
         {
            return StatusCode(500, new ApiError("Failed to create a link."));
         }

         // TODO: Continue actions according to sequence diagram

         var newLinkUrl = Url.Link(nameof(EndpointLinksController.CreateLinkAsync), null);
         newLinkUrl = newLinkUrl + "/" + endpointLinkViewModel.LinkId;

         return Created(newLinkUrl, endpointLinkViewModel);
      }
   }
}
