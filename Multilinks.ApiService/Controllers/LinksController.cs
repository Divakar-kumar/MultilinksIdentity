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

      public LinksController(IUserInfoService userInfoService, ILinkService linkService)
      {
         _userInfoService = userInfoService;
         _linkService = linkService;
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

         /* Check user is source endpoint owner. */
         var isOwner = await _linkService.CheckEndpointOwnedByUserAsync(_userInfoService.UserId, sourceId, ct);

         if(!isOwner)
         {
            return BadRequest(new ApiError("Cannot create a link from a device you do not own."));
         }

         // Do more validation and actions according to sequence diagram

         var newLinkUrl = Url.Link(nameof(LinksController.CreateLinkAsync), null);
         newLinkUrl = newLinkUrl + "/" + "linkId";

         return Created(newLinkUrl, null);
      }
   }
}
