/*************************************************************************************************
 *    Class:      Endpoint
 * 
 *    Summary:    This is the view model version of EndpointIdentity. It will contains properties
 *                related to EndpointIdentity that is useful to the client.
 *                The EndpointId is used to retreived the info from the data store.
 * 
 *************************************************************************************************/

using Multilinks.ApiService.Infrastructure;
using System;
using static Multilinks.ApiService.Models.EndpointEntity;

namespace Multilinks.ApiService.Models
{
   public class EndpointViewModel : Resource
   {
      public Guid ServiceAreaId { get; set; }

      public Guid CreatorId { get; set; }

      [Sortable]
      public bool IsCloudConnected { get; set; }

      [Sortable]
      public bool IsGateway { get; set; }

      public CommsDirectionCapabilities DirectionCapability { get; set; }

      [Sortable(Default = true)]
      public string Name { get; set; }

      public string Description { get; set; }
   }
}
