using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Multilinks.ApiService.Models;
using Microsoft.AspNetCore.Identity;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Multilinks.DataService.Entities;

namespace Multilinks.ApiService.Services
{
   public class UserService : IUserService
   {
      private readonly UserManager<UserEntity> _userManager;

      public UserService(UserManager<UserEntity> userManager)
      {
         _userManager = userManager;
      }

      public async Task<PagedResults<UserViewModel>> GetUsersAsync(
         PagingOptions pagingOptions,
         SortOptions<UserViewModel, UserEntity> sortOptions,
         SearchOptions<UserViewModel, UserEntity> searchOptions,
         CancellationToken ct)
      {
         IQueryable<UserEntity> query = _userManager.Users;
         query = searchOptions.Apply(query);
         query = sortOptions.Apply(query);

         var size = await query.CountAsync(ct);

         var items = await query
             .Skip(pagingOptions.Offset.Value)
             .Take(pagingOptions.Limit.Value)
             .ProjectTo<UserViewModel>()
             .ToArrayAsync(ct);

         return new PagedResults<UserViewModel>
         {
            Items = items,
            TotalSize = size
         };
      }
   }
}
