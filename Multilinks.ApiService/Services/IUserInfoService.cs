namespace Multilinks.ApiService.Services
{
   public interface IUserInfoService
   {
      string UserId { get; set; }
      string FirstName { get; set; }
      string LastName { get; set; }
      string Role { get; set; }

      /* The following are client details on behalf of the user */
      string ClientId { get; set; }
      string ClientType { get; set; }
   }
}
