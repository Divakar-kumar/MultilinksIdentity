using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Net.Http.Headers;

namespace Multilinks.ApiService.Services
{
   public class UserInfoService : IUserInfoService
   {
      private readonly IHttpContextAccessor _httpContextAccessor;

      public Guid UserId { get; }
      public string FirstName { get; }
      public string LastName { get; }
      public string Role { get; }
      public string ClientId { get; }
      public string ClientType { get; }

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
