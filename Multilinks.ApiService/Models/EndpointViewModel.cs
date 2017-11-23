/*************************************************************************************************
 *    Class:      Endpoint
 * 
 *    Summary:    This is the view model version of EndpointIdentity. It will contains properties
 *                related to EndpointIdentity that is useful to the client.
 *                The EndpointId is used to retreived the info from the data store.
 * 
 *************************************************************************************************/

using System;
using static Multilinks.ApiService.Models.EndpointEntity;

namespace Multilinks.ApiService.Models
{
   public class EndpointViewModel : Resource
   {
      public Guid ServiceAreaId { get; set; }

      public Guid CreatorId { get; set; }

      public bool IsCloudConnected { get; set; }

      public bool IsGateway { get; set; }

      public CommsDirectionCapabilities DirectionCapability { get; set; }

      public string Name { get; set; }

      public string Description { get; set; }
   }
}
