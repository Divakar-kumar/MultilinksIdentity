using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Multilinks.ApiService.Services
{
   public class UserInfoService : IUserInfoService
   {
      private readonly IHttpContextAccessor _httpContextAccessor;

      public Guid UserId { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string Role { get; set; }
      public string ClientId { get; set; }
      public string ClientType { get; set; }

      public UserInfoService(IHttpContextAccessor httpContextAccessor)
      {
         /* Service is scoped, created once for each request => we only need
          * to fetch the info in the constructor. */
         _httpContextAccessor = httpContextAccessor
             ?? throw new ArgumentNullException(nameof(httpContextAccessor));

         var currentContext = _httpContextAccessor.HttpContext;
         if(currentContext == null || !currentContext.User.Identity.IsAuthenticated)
         {
            return;
         }

         UserId = new Guid(currentContext
             .User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value);

         FirstName = currentContext.User
             .Claims.FirstOrDefault(c => c.Type == "given_name")?.Value;

         LastName = currentContext
             .User.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value;

         Role = currentContext
           .User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

         ClientId = currentContext
            .User.Claims.FirstOrDefault(c => c.Type == "client_id")?.Value;

         ClientType = currentContext
           .User.Claims.FirstOrDefault(c => c.Type == "client_Type")?.Value;
      }
   }
}
