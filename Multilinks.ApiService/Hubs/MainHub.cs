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
      private readonly IEndpointService _endpointService;

      public MainHub(IHubConnectionService hubConnectionService,
         IEndpointService endpointService)
      {
         _hubConnectionService = hubConnectionService;
         _endpointService = endpointService;
      }

      public override async Task OnConnectedAsync()
      {
         var endpointId = Context.GetHttpContext().Request.Query["ep"];
         var creatorId = Context.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

         if(string.IsNullOrEmpty(endpointId) || string.IsNullOrEmpty(creatorId))
         {
            Context.Abort();
         }

         var isCreator = await _endpointService.CheckEndpointIsCreatedByUser(Guid.Parse(endpointId),
            Guid.Parse(creatorId),
            Context.ConnectionAborted);

         if(!isCreator)
         {
            Context.Abort();
         }

         var connectionReferenceCreated = await _hubConnectionService.CreateHubConnectionReferenceAsync(Guid.Parse(endpointId),
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