using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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
      private readonly INotificationService _notificationService;
      private readonly IHubContext<MainHub, IMainHub> _hubContext;
      private readonly PagingOptions _defaultPagingOptions;

      public EndpointLinksController(IUserInfoService userInfoService,
         IEndpointLinkService linkService,
         IEndpointService endpointService,
         INotificationService notificationService,
         IHubContext<MainHub, IMainHub> hubContext,
         IOptions<PagingOptions> defaultPagingOptions)
      {
         _userInfoService = userInfoService;
         _linkService = linkService;
         _endpointService = endpointService;
         _notificationService = notificationService;
         _hubContext = hubContext;
         _defaultPagingOptions = defaultPagingOptions.Value;
      }

      // GET api/endpointlinks/id/{linkId}
      [HttpGet("id/{linkId}", Name = nameof(GetEndpointLinkByIdAsync))]
      [ResponseCache(CacheProfileName = "Resource")]
      [Etag]
      public async Task<IActionResult> GetEndpointLinkByIdAsync(Guid linkId, CancellationToken ct)
      {
         var endpointLink = await _linkService.GetLinkByIdAsync(linkId, ct);

         if(endpointLink == null)
         {
            return NotFound();
         }

         if (_userInfoService.UserId != endpointLink.SourceEndpoint.Owner.IdentityId)
         {
            return StatusCode(403, new ApiError("Access to the requested link is denied."));
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

      // GET api/endpointlinks/confirmed/source-id/{sourceId}
      [HttpGet("confirmed/source-id/{sourceId}", Name = nameof(GetEndpointLinksConfirmedAsync))]
      [ResponseCache(CacheProfileName = "Collection")]
      [Etag]
      public async Task<IActionResult> GetEndpointLinksConfirmedAsync(Guid sourceId,
         [FromQuery] PagingOptions pagingOptions,
         CancellationToken ct)
      {
         if(!ModelState.IsValid)
            return BadRequest(new ApiError(ModelState));

         pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
         pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

         var links = await _linkService.GetEndpointLinksConfirmedAsync(sourceId, _userInfoService.UserId, pagingOptions, ct);

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

      // PATCH api/endpointlinks/confirmation/{linkId}
      [HttpPatch("confirmation/{linkId}", Name = nameof(ConfirmEndpointLinkByIdAsync))]
      [ResponseCache(CacheProfileName = "Resource")]
      public async Task<IActionResult> ConfirmEndpointLinkByIdAsync(Guid linkId,
         [FromBody] JsonPatchDocument<ConfirmEndpointLinkForm> jsonPatchDocument,
         CancellationToken ct)
      {
         if(jsonPatchDocument == null)
         {
            return BadRequest();
         }

         var existingLink = await _linkService.GetLinkByIdAsync(linkId, ct);

         if((existingLink == null) || existingLink.AssociatedEndpoint.Owner.IdentityId != _userInfoService.UserId)
         {
            return NotFound();
         }

         var linkToPatch = Mapper.Map<ConfirmEndpointLinkForm>(existingLink);

         jsonPatchDocument.ApplyTo(linkToPatch, ModelState);

         if(!ModelState.IsValid)
         {
            return new UnprocessableEntityObjectResult(ModelState);
         }

         if(!TryValidateModel(linkToPatch))
         {
            return new UnprocessableEntityObjectResult(ModelState);
         }

         var linkUpdated = await _linkService.UpdateLinkStatusByIdAsync(linkId, linkToPatch.Confirmed, ct);

         if(!linkUpdated)
         {
            return StatusCode(500, new ApiError("Link failed to be updated."));
         }

         var notification = new NotificationEntity
         {
            Id = Guid.NewGuid(),
            RecipientEndpoint = existingLink.SourceEndpoint,
            NotificationType = NotificationEntity.Type.LinkRequestAccepted,
            Message = $"Request to link with {existingLink.AssociatedEndpoint.Owner.OwnerName}'s {existingLink.AssociatedEndpoint.Name} ({existingLink.AssociatedEndpoint.EndpointId}) was accepted."
         };

         var notificationCreated = await _notificationService.CreateNotificationAsync(notification, ct);

         if(existingLink.SourceEndpoint.HubConnection.Connected)
         {
            await _hubContext.Clients.Client(existingLink.SourceEndpoint.HubConnection.ConnectionId)
               .LinkConfirmationReceived(existingLink.LinkId.ToString(),
                                         existingLink.AssociatedEndpoint.Name,
                                         existingLink.AssociatedEndpoint.Owner.OwnerName,
                                         existingLink.AssociatedEndpoint.HubConnection.Connected);

            if(notificationCreated)
            {
               await _hubContext.Clients.Client(existingLink.SourceEndpoint.HubConnection.ConnectionId)
                  .NotificationReceived(notification.Id.ToString(),
                     notification.NotificationType,
                     notification.Message,
                     notification.Hidden);
            }
         }

         return NoContent();
      }

      // DELETE api/endpointlinks/confirmation/{linkId}
      [HttpDelete("confirmation/{linkId}", Name = nameof(DeclineEndpointLinkByIdAsync))]
      [ResponseCache(CacheProfileName = "Resource")]
      public async Task<IActionResult> DeclineEndpointLinkByIdAsync(Guid linkId, CancellationToken ct)
      {
         var endpointLink = await _linkService.GetLinkByIdAsync(linkId, ct);

         if(endpointLink == null)
         {
            return NoContent();
         }

         if(_userInfoService.UserId != endpointLink.AssociatedEndpoint.Owner.IdentityId)
         {
            return StatusCode(403, new ApiError("No permission to denied the requested link."));
         }

         var deleted = await _linkService.DeleteLinkByIdAsync(linkId, _userInfoService.UserId, ct);

         if(!deleted)
            return StatusCode(500, new ApiError("Link failed to be deleted."));

         var notification = new NotificationEntity
         {
            Id = Guid.NewGuid(),
            RecipientEndpoint = endpointLink.SourceEndpoint,
            NotificationType = NotificationEntity.Type.LinkRequestDenied,
            Message = $"Request to link with {endpointLink.AssociatedEndpoint.Owner.OwnerName}'s {endpointLink.AssociatedEndpoint.Name} ({endpointLink.AssociatedEndpoint.EndpointId}) was denied."
         };

         var createdSuccess = await _notificationService.CreateNotificationAsync(notification, ct);

         if (createdSuccess && endpointLink.SourceEndpoint.HubConnection.Connected)
         {
            await _hubContext.Clients.Client(endpointLink.SourceEndpoint.HubConnection.ConnectionId)
               .NotificationReceived(notification.Id.ToString(),
                  notification.NotificationType,
                  notification.Message,
                  notification.Hidden);
         }

         return NoContent();
      }
   }
}
