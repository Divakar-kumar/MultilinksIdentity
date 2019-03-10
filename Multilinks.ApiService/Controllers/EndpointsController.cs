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
   public class EndpointsController : Controller
   {
      private readonly IEndpointService _endpointService;
      private readonly IUserInfoService _userInfoService;
      private readonly PagingOptions _defaultPagingOptions;

      public EndpointsController(IEndpointService endpointService,
         IUserInfoService userInfoService,
         IOptions<PagingOptions> defaultPagingOptions)
      {
         _endpointService = endpointService;
         _userInfoService = userInfoService;
         _defaultPagingOptions = defaultPagingOptions.Value;
      }

      // GET api/endpoints/id/{endpointId}
      [HttpGet("id/{endpointId}", Name = nameof(GetEndpointByIdAsync))]
      [ResponseCache(CacheProfileName = "Resource")]
      [Etag]
      public async Task<IActionResult> GetEndpointByIdAsync(Guid endpointId, CancellationToken ct)
      {
         var endpoint = await _endpointService.GetEndpointByIdAsync(endpointId, ct);

         if(endpoint == null || endpoint.Owner.IdentityId != _userInfoService.UserId)
            return NotFound();

         var endpointViewModel = Mapper.Map<EndpointViewModel>(endpoint);

         if(!Request.GetEtagHandler().NoneMatch(endpointViewModel))
         {
            return StatusCode(304, endpointViewModel);
         }

         return Ok(endpointViewModel);
      }

      // GET api/endpoints/login/{name}
      [HttpGet("login/{name}", Name = nameof(EndpointLoginByNameAsync))]
      [ResponseCache(CacheProfileName = "Resource")]
      [Etag]
      public async Task<IActionResult> EndpointLoginByNameAsync(string name, CancellationToken ct)
      {
         var endpoint = await _endpointService.GetEndpointByNameAsync(name, _userInfoService.UserId, ct);

         if(endpoint == null)
         {
            var client = new EndpointClientEntity
            {
               ClientId = _userInfoService.ClientId,
               ClientType = _userInfoService.ClientType
            };

            var owner = new EndpointOwnerEntity
            {
               IdentityId = _userInfoService.UserId,
               OwnerName = _userInfoService.Name
            };

            endpoint = await _endpointService.CreateEndpointAsync(name, client, owner, ct);

            if(endpoint == null)
               return BadRequest(new ApiError("Cannot login, device cannot be created"));
         }

         var endpointViewModel = Mapper.Map<EndpointViewModel>(endpoint);

         if(!Request.GetEtagHandler().NoneMatch(endpointViewModel))
         {
            return StatusCode(304, endpointViewModel);
         }

         return Ok(endpointViewModel);
      }

      // GET api/endpoints/my-endpoints
      [HttpGet("my-endpoints", Name = nameof(GetEndpointsAsync))]
      [ResponseCache(CacheProfileName = "Collection")]
      [Etag]
      public async Task<IActionResult> GetEndpointsAsync(
         [FromQuery] PagingOptions pagingOptions,
         [FromQuery] SortOptions<EndpointViewModel, EndpointEntity> sortOptions,
         [FromQuery] SearchOptions<EndpointViewModel, EndpointEntity> searchOptions,
         CancellationToken ct)
      {
         if(!ModelState.IsValid)
            return BadRequest(new ApiError(ModelState));

         pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
         pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

         var endpoints = await _endpointService.GetEndpointsByOwnerIdAsync(_userInfoService.UserId,
                                                                           pagingOptions,
                                                                           sortOptions,
                                                                           searchOptions,
                                                                           ct);

         /* What we get from data repository are endpoints, but what we want to return to users are endpoints view model. */
         var endpointsViewModel = new PagedResults<EndpointViewModel>()
         {
            Items = Mapper.Map<IEnumerable<EndpointEntity>, IEnumerable<EndpointViewModel>>(endpoints.Items),
            TotalSize = endpoints.TotalSize
         };

         var collection = PagedCollection<EndpointViewModel>.Create<EndpointsResponse>(Link.ToCollection(nameof(GetEndpointsAsync)),
                                                                                       endpointsViewModel.Items.ToArray(),
                                                                                       endpointsViewModel.TotalSize,
                                                                                       pagingOptions);

         if(!Request.GetEtagHandler().NoneMatch(collection))
         {
            return StatusCode(304, collection);
         }

         return Ok(collection);
      }

      // DELETE api/endpoints/id/{endpointId}
      [HttpDelete("id/{endpointId}", Name = nameof(DeleteEndpointByIdAsync))]
      [ResponseCache(CacheProfileName = "Resource")]
      public async Task<IActionResult> DeleteEndpointByIdAsync(Guid endpointId, CancellationToken ct)
      {
         var existingEndpoint = await _endpointService.GetEndpointByIdAsync(endpointId, ct);

         if((existingEndpoint == null) || existingEndpoint.Owner.IdentityId != _userInfoService.UserId)
         {
            return NoContent();
         }

         /* We are the owner of this endpoint so go ahead and delete it. */
         var endpointDeleted = await _endpointService.DeleteEndpointByIdAsync(endpointId, ct);

         /* Something went wrong while trying to delete this endpoint. */
         if(!endpointDeleted)
            return StatusCode(500, new ApiError("Device failed to be deleted"));

         return NoContent();
      }

      //// PATCH api/endpoints/id/{endpointId}
      //[HttpPatch("id/{endpointId}", Name = nameof(UpdateEndpointByIdAsync))]
      //[ResponseCache(CacheProfileName = "Resource")]
      //public async Task<IActionResult> UpdateEndpointByIdAsync(Guid endpointId,
      //   [FromBody] JsonPatchDocument<UpdateEndpointForm> jsonPatchDocument,
      //   CancellationToken ct)
      //{
      //   if(jsonPatchDocument == null)
      //   {
      //      return BadRequest();
      //   }

      //   var existingEndpoint = await _endpointService.GetEndpointByIdAsync(endpointId, ct);

      //   if((existingEndpoint == null) || existingEndpoint.Owner.IdentityId != _userInfoService.UserId)
      //   {
      //      return NotFound();
      //   }

      //   var endpointToPatch = Mapper.Map<UpdateEndpointForm>(existingEndpoint);

      //   jsonPatchDocument.ApplyTo(endpointToPatch, ModelState);

      //   if(!ModelState.IsValid)
      //   {
      //      return new UnprocessableEntityObjectResult(ModelState);
      //   }

      //   if(!TryValidateModel(endpointToPatch))
      //   {
      //      return new UnprocessableEntityObjectResult(ModelState);
      //   }

      //   var endpointUpdated = await _endpointService.UpdateEndpointByIdAsync(endpointId,
      //                                                                        endpointToPatch.Description,
      //                                                                        ct);

      //   if(!endpointUpdated)
      //   {
      //      return BadRequest(new ApiError("Device failed to be updated"));
      //   }

      //   return NoContent();
      //}
   }
}
