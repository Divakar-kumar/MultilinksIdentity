using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Multilinks.ApiService.Entities;
using Multilinks.ApiService.Infrastructure;
using Multilinks.ApiService.Models;
using Multilinks.ApiService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Controllers
{
   [Route("api/[controller]")]
   [ApiVersion("1.0")]
   [Authorize]
   public class NotificationsController : Controller
   {
      private readonly IUserInfoService _userInfoService;
      private readonly INotificationService _notificationService;
      private readonly PagingOptions _defaultPagingOptions;

      public NotificationsController(IUserInfoService userInfoService,
         INotificationService notificationService,
         IOptions<PagingOptions> defaultPagingOptions)
      {
         _userInfoService = userInfoService;
         _notificationService = notificationService;
         _defaultPagingOptions = defaultPagingOptions.Value;
      }

      // GET api/notifications/id/{id}
      [HttpGet("id/{id}", Name = nameof(GetNotificationByIdAsync))]
      [ResponseCache(CacheProfileName = "Resource")]
      [Etag]
      public async Task<IActionResult> GetNotificationByIdAsync(Guid id, CancellationToken ct)
      {
         var notification = await _notificationService.GetNotificationByIdAsync(id, ct);

         if(notification == null)
         {
            return NotFound();
         }

         if(_userInfoService.UserId != notification.RecipientEndpoint.Owner.IdentityId)
         {
            return StatusCode(403, new ApiError("Access to the requested notification is denied."));
         }

         var notificationViewModel = Mapper.Map<EndpointLinkViewModel>(notification);

         if(!Request.GetEtagHandler().NoneMatch(notificationViewModel))
         {
            return StatusCode(304, notificationViewModel);
         }

         return Ok(notificationViewModel);
      }

      // GET api/notifications/new/{endpointId}
      [HttpGet("new/{endpointId}", Name = nameof(GetNewNotificationsAsync))]
      [ResponseCache(CacheProfileName = "Collection")]
      [Etag]
      public async Task<IActionResult> GetNewNotificationsAsync(Guid endpointId,
         [FromQuery] PagingOptions pagingOptions,
         CancellationToken ct)
      {
         pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
         pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

         var notifications = await _notificationService.GetNewNotificationsAsync(endpointId, _userInfoService.UserId, pagingOptions, ct);

         var notificationsCollection = new PagedResults<NotificationViewModel>()
         {
            Items = Mapper.Map<IEnumerable<NotificationEntity>, IEnumerable<NotificationViewModel>>(notifications.Items),
            TotalSize = notifications.TotalSize
         };

         var collection = PagedCollection<NotificationViewModel>.Create<NotificationsCollection>(Link.ToCollection(nameof(GetNotificationByIdAsync)),
                                                                                       notificationsCollection.Items.ToArray(),
                                                                                       notificationsCollection.TotalSize,
                                                                                       pagingOptions);

         if(!Request.GetEtagHandler().NoneMatch(collection))
         {
            return StatusCode(304, collection);
         }

         return Ok(collection);
      }

      // PATCH api/notifications/id/{id}
      [HttpPatch("id/{id}", Name = nameof(UpdateNotificationByIdAsync))]
      [ResponseCache(CacheProfileName = "Resource")]
      public async Task<IActionResult> UpdateNotificationByIdAsync(Guid id,
         [FromBody] JsonPatchDocument<UpdateNotificationForm> jsonPatchDocument,
         CancellationToken ct)
      {
         if(jsonPatchDocument == null)
         {
            return BadRequest();
         }

         var existingNotification = await _notificationService.GetNotificationByIdAsync(id, ct);

         if((existingNotification == null) || existingNotification.RecipientEndpoint.Owner.IdentityId != _userInfoService.UserId)
         {
            return NotFound();
         }

         var linkToPatch = Mapper.Map<UpdateNotificationForm>(existingNotification);

         jsonPatchDocument.ApplyTo(linkToPatch, ModelState);

         if(!ModelState.IsValid)
         {
            return new UnprocessableEntityObjectResult(ModelState);
         }

         if(!TryValidateModel(linkToPatch))
         {
            return new UnprocessableEntityObjectResult(ModelState);
         }

         var notificationUpdated = await _notificationService.UpdateHiddenStatusByIdAsync(id, linkToPatch.Hidden, ct);

         if(!notificationUpdated)
         {
            return StatusCode(500, new ApiError("Link failed to be updated."));
         }

         return NoContent();
      }
   }
}
