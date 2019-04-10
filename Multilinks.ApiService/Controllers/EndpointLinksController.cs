using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Multilinks.ApiService.Entities;
using Multilinks.ApiService.Hubs;
using Multilinks.ApiService.Hubs.Interfaces;
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
      private readonly IHubContext<MainHub, IMainHub> _hubContext;
      private readonly PagingOptions _defaultPagingOptions;

      public EndpointLinksController(IUserInfoService userInfoService,
         IEndpointLinkService linkService,
         IEndpointService endpointService,
         IHubContext<MainHub, IMainHub> hubContext,
         IOptions<PagingOptions> defaultPagingOptions)
      {
         _userInfoService = userInfoService;
         _linkService = linkService;
         _endpointService = endpointService;
         _hubContext = hubContext;
         _defaultPagingOptions = defaultPagingOptions.Value;
      }

      // GET api/endpointlinks/id/{linkId}
      [HttpGet("id/{linkId}", Name = nameof(GetEndpointLinkByIdAsync))]
      [ResponseCache(CacheProfileName = "Resource")]
      [Etag]
      public async Task<IActionResult> GetEndpointLinkByIdAsync(Guid linkId, CancellationToken ct)
      {
         // TODO: Need more validation to ensure user has access to this link
         var endpointLink = await _linkService.GetLinkByIdAsync(linkId, ct);

         if(endpointLink == null)
         {
            return NotFound();
         }

         var endpointLinkViewModel = Mapper.Map<EndpointLinkViewModel>(endpointLink);

         if(!Request.GetEtagHandler().NoneMatch(endpointLinkViewModel))
         {
            return StatusCode(304, endpointLinkViewModel);
         }

         return Ok(endpointLinkViewModel);
      }

      // GET api/endpointlinks/pending/{endpointId}
      [HttpGet("pending/{endpointId}", Name = nameof(GetEndpointLinksPendingAsync))]
      [ResponseCache(CacheProfileName = "Collection")]
      [Etag]
      public async Task<IActionResult> GetEndpointLinksPendingAsync(Guid endpointId,
         [FromQuery] PagingOptions pagingOptions,
         CancellationToken ct)
      {
         if(!ModelState.IsValid)
            return BadRequest(new ApiError(ModelState));

         pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
         pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

         var links = await _linkService.GetEndpointLinksPendingAsync(endpointId, _userInfoService.UserId, pagingOptions, ct);

         var endpointsLinkViewModel = new PagedResults<EndpointLinkViewModel>()
         {
            Items = Mapper.Map<IEnumerable<EndpointLinkEntity>, IEnumerable<EndpointLinkViewModel>>(links.Items),
            TotalSize = links.TotalSize
         };

         var collection = PagedCollection<EndpointLinkViewModel>.Create<EndpointsLinkCollection>(Link.ToCollection(nameof(GetEndpointLinkByIdAsync)),
                                                                                       endpointsLinkViewModel.Items.ToArray(),
                                                                                       endpointsLinkViewModel.TotalSize,
                                                                                       pagingOptions);

         if(!Request.GetEtagHandler().NoneMatch(collection))
         {
            return StatusCode(304, collection);
         }

         return Ok(collection);
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
            return BadRequest(new ApiError("Link to specified device already exists."));
         }

         var sourceEndpoint = await _endpointService.GetEndpointByIdAsync(sourceId, ct);

         if(sourceEndpoint == null)
         {
            return BadRequest(new ApiError("Requesting device is invalid."));
         }

         if(sourceEndpoint.Owner.IdentityId != _userInfoService.UserId)
         {
            return BadRequest(new ApiError("Cannot create a link from a device not created by current user."));
         }

         var destinationEndpoint = await _endpointService.GetEndpointByIdAsync(destinationId, ct);

         if(destinationEndpoint == null)
         {
            return BadRequest(new ApiError("Target device is invalid."));
         }

         endpointLink = await _linkService.CreateLinkAsync(sourceEndpoint, destinationEndpoint, ct);

         if(endpointLink == null)
         {
            return StatusCode(500, new ApiError("Failed to create a link."));
         }

         if(endpointLink.AssociatedEndpoint.HubConnection.Connected)
         {
            await _hubContext.Clients.Client(endpointLink.AssociatedEndpoint.HubConnection.ConnectionId)
               .LinkRequestReceived(endpointLink.LinkId.ToString(),
                                    endpointLink.SourceEndpoint.Name,
                                    endpointLink.SourceEndpoint.Owner.OwnerName);
         }

         var newLinkUrl = Url.Link(nameof(EndpointLinksController.CreateLinkAsync), null);
         newLinkUrl = newLinkUrl + "/" + endpointLink.LinkId;

         return Created(newLinkUrl, null);
      }

      // DELETE api/endpointlinks/id/{linkId}
      [HttpDelete("id/{linkId}", Name = nameof(DeleteEndpointLinkByIdAsync))]
      [ResponseCache(CacheProfileName = "Resource")]
      public async Task<IActionResult> DeleteEndpointLinkByIdAsync(Guid linkId, CancellationToken ct)
      {
         var deleted = await _linkService.DeleteLinkByIdAsync(linkId, _userInfoService.UserId, ct);

         if(!deleted)
            return StatusCode(500, new ApiError("Link failed to be deleted"));

         return NoContent();
      }
   }
}
