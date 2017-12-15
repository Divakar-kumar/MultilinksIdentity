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

namespace Multilinks.ApiService.Controllers
{
   [Route("api/[controller]")]
   [ApiVersion("1.0")]
   [Authorize]
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
      [HttpGet(Name = nameof(GetVisibleUsersAsync))]
      [AllowAnonymous]
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

         var collection = PagedCollection<UserViewModel>.Create(Link.To(nameof(GetVisibleUsersAsync)),
                                                                users.Items.ToArray(),
                                                                users.TotalSize,
                                                                pagingOptions);

         return Ok(collection);
      }

      // GET api/users/{userId}
      [HttpGet("{userId}", Name = nameof(GetUserByIdAsync))]
      public Task<IActionResult> GetUserByIdAsync(Guid userId, CancellationToken ct)
      {
         // TODO is userId the current user's ID?
         // If so, return myself.
         // If not, only Admin roles should be able to view arbitrary users.
         throw new NotImplementedException();
      }
   }
}
