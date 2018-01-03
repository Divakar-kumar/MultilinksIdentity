namespace Multilinks.ApiService.Models
{
   public class UsersResponse : PagedCollection<UserViewModel>
   {
      public Form QueryForm { get; set; }
   }
}
