using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Multilinks.ApiService.Models;
using System.Threading;
using Microsoft.Extensions.Options;
using Multilinks.ApiService.Services;
using System.Linq;
using System;
using Multilinks.DataService.Entities;
using Multilinks.ApiService.Infrastructure;

namespace Multilinks.ApiService.Controllers
{
   [Route("api/[controller]")]
   [ApiVersion("1.0")]
   //[Authorize]
   public class UsersController : Controller
   {
      private readonly IUserService _userService;
      private readonly PagingOptions _defaultPagingOptions;

      public UsersController(IUserService userService, IOptions<PagingOptions> defaultPagingOptions)
      {
         _userService = userService;
         _defaultPagingOptions = defaultPagingOptions.Value;
      }

      // GET api/users
      // TODO: Admin only
      [HttpGet(Name = nameof(GetVisibleUsersAsync))]
      [ResponseCache(CacheProfileName = "Collection")]
      [Etag]
      public async Task<IActionResult> GetVisibleUsersAsync(
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SortOptions<UserViewModel, UserEntity> sortOptions,
            [FromQuery] SearchOptions<UserViewModel, UserEntity> searchOptions,
            CancellationToken ct)
      {
         if(!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

         pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
         pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

         // TODO: Authorization check. Is the user an admin?
         var users = await _userService.GetUsersAsync(pagingOptions, sortOptions, searchOptions, ct);

         var collection = PagedCollection<UserViewModel>.Create<UsersResponse>(Link.To(nameof(GetVisibleUsersAsync)),
                                                                               users.Items.ToArray(),
                                                                               users.TotalSize,
                                                                               pagingOptions);

         collection.QueryForm = FormMetadata.FromResource<UserViewModel>(Link.ToForm(nameof(GetVisibleUsersAsync),
                                                                             null,
                                                                             Link.GetMethod,
                                                                             Form.QueryRelation));

         if(!Request.GetEtagHandler().NoneMatch(collection))
         {
            return StatusCode(304, collection);
         }

         return Ok(collection);
      }

      // GET api/users/{userId}
      // TODO: Admin only
      [HttpGet("{userId}", Name = nameof(GetUserByIdAsync))]
      public Task<IActionResult> GetUserByIdAsync(Guid userId, CancellationToken ct)
      {
         throw new NotImplementedException();
      }
   }
}
