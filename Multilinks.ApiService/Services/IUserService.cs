using Multilinks.ApiService.Models;
using Multilinks.DataService.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Services
{
   public interface IUserService
   {
      Task<PagedResults<UserViewModel>> GetUsersAsync(PagingOptions pagingOptions,
                                                      SortOptions<UserViewModel, UserEntity> sortOptions,
                                                      SearchOptions<UserViewModel, UserEntity> searchOptions,
                                                      CancellationToken ct);
   }
}
