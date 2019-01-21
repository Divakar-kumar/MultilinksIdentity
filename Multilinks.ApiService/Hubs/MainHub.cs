using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Multilinks.ApiService.Services;
using System;
using System.Linq;
using System.Threading;
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
         var endpointId = Context.GetHttpContext().Request.Query["ep"];

         if(string.IsNullOrEmpty(endpointId))
         {
            Context.Abort();
         }

         var connectionReferenceCreated = await _hubConnectionService.CreateHubConnectionReferenceAsync(
            Guid.Parse(endpointId),
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

         var connectionReferenceDeleted = await _hubConnectionService.DeleteHubConnectionReferenceAsync(
            Context.ConnectionId,
            CancellationToken.None);

         await base.OnDisconnectedAsync(exception);
      }
   }
}