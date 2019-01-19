using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Hubs
{
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   public class MainHub : Hub
   {
      public override async Task OnConnectedAsync()
      {
         Context.Items["Role"] = Context.User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
         await base.OnConnectedAsync();
      }
   }
}