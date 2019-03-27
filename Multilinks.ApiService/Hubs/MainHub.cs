using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Multilinks.ApiService.Hubs.Interfaces;
using Multilinks.ApiService.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Hubs
{
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   public class MainHub : Hub<IMainHub>
   {
      private readonly IHubConnectionService _hubConnectionService;

      public MainHub(IHubConnectionService hubConnectionService)
      {
         _hubConnectionService = hubConnectionService;
      }

      public override async Task OnConnectedAsync()
      {
         var endpointId = Context.GetHttpContext().Request.Query["ep"];
         var ownerId = Context.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

         if(string.IsNullOrEmpty(endpointId) || string.IsNullOrEmpty(ownerId))
         {
            Context.Abort();
         }

         var connectionReferenceCreated = await _hubConnectionService.ConnectHubConnectionReferenceAsync(
            Guid.Parse(endpointId),
            Guid.Parse(ownerId),
            Context.ConnectionId,
            Context.ConnectionAborted);

         if(!connectionReferenceCreated)
         {
            Context.Abort();
         }

         await base.OnConnectedAsync();
      }

      public override async Task OnDisconnectedAsync(Exception exception)
      {

         var connectionReferenceDeleted = await _hubConnectionService.DisconnectHubConnectionReferenceAsync(
            Context.ConnectionId,
            CancellationToken.None);

         await base.OnDisconnectedAsync(exception);
      }
   }
}