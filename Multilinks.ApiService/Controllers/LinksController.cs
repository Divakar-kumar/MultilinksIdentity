using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multilinks.ApiService.Models;
using Multilinks.ApiService.Services;

namespace Multilinks.ApiService.Controllers
{
   [Route("api/[controller]")]
   [ApiVersion("1.0")]
   [Authorize]
   public class LinksController : Controller
   {
      private readonly IUserInfoService _userInfoService;
      private readonly ILinkService _linkService;
      private readonly IEndpointService _endpointService;

      public LinksController(IUserInfoService userInfoService,
         ILinkService linkService,
         IEndpointService endpointService)
      {
         _userInfoService = userInfoService;
         _linkService = linkService;
         _endpointService = endpointService;
      }

      [HttpPost(Name = nameof(CreateLinkAsync))]
      [ResponseCache(CacheProfileName = "Resource")]
      public async Task<IActionResult> CreateLinkAsync([FromBody] NewLinkForm newLink,
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

         var sourceEndpoint = await _endpointService.GetEndpointByIdAsync(sourceId, ct);

         if(sourceEndpoint == null)
         {
            return BadRequest(new ApiError("Source device is invalid."));
         }

         if(sourceEndpoint.CreatorId != _userInfoService.UserId)
         {
            return BadRequest(new ApiError("Cannot create a link from a device not created by current user."));
         }

         var destinationEndpoint = await _endpointService.GetEndpointByIdAsync(destinationId, ct);

         if(destinationEndpoint == null)
         {
            return BadRequest(new ApiError("Destination device is invalid."));
         }

         // Do more validation and actions according to sequence diagram

         var newLinkUrl = Url.Link(nameof(LinksController.CreateLinkAsync), null);
         newLinkUrl = newLinkUrl + "/" + "linkId";

         return Created(newLinkUrl, null);
      }
   }
}
