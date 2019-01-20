using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Multilinks.ApiService.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Hubs
{
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   public class MainHub : Hub
   {
      private readonly IHubConnectionService _hubConnectionService;

      public MainHub(IHubConnectionService hubConnectionService)
      {
         _hubConnectionService = hubConnectionService;
      }

      public override async Task OnConnectedAsync()
      {
         Context.Items["Role"] = Context.User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
         Context.Items["EndpointId"] = Context.GetHttpContext().Request.Query["ep"];

         var connectionReferenceUpdated = await _hubConnectionService.UpdateHubConnectionReferenceAsync(
            Guid.Parse(Context.Items["EndpointId"].ToString()),
            Context.ConnectionId,
            Context.ConnectionAborted);

         if(!connectionReferenceUpdated)
         {
            Context.Abort();
         }

         await base.OnConnectedAsync();
      }
   }
}