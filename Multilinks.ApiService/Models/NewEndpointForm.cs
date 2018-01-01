/*************************************************************************************************
 *    Class:      NewEndpointForm
 * 
 *    Summary:    This is the form used in the post request to create a new
 *                endpoint. For more information about endpoints refer to EndpointEntity class.
 *             
 *    Property:   1. A create new endpoint request can be posted from the client or the gateway.
 *                2. When a request is from the client, the SAID will be the default SAID for
 *                   cloud-connected devices.
 *                3. When a request is from the gateway, the SAID will be the same as the SAID
 *                   of the gateway (i.e. which is also the same as the EPID of the gateway).
 *                4. The CID will be the ApplicationUserId of the logged in user.
 *                5. If this endpoint is cloud-connected and is not a gateway, then it is
 *                   an endpoint hosting a client.
 *                6. If this endpoint is cloud-connected and is a gateway, then it is an
 *                   endpoint hosting a gateway.
 *                7. If this endpoint is not cloud-connected and is not a gateway, then it is
 *                   an endpoint managed by a gateway as defined by it's SAID.
 * 
 *************************************************************************************************/

using System;
using System.ComponentModel.DataAnnotations;
using static Multilinks.DataService.Entities.EndpointEntity;

namespace Multilinks.ApiService.Models
{
   public class NewEndpointForm
   {
      [Required]
      [Display(Name = "serviceAreaId", Description = "Service area Id")]
      public Guid ServiceAreaId { get; set; }

      [Required]
      [Display(Name = "creatorId", Description = "Creator Id")]
      public Guid CreatorId { get; set; }

      [Required]
      [Display(Name = "isCloudConnected", Description = "Is endpoint cloud-connected")]
      public bool IsCloudConnected { get; set; }

      [Required]
      [Display(Name = "isGateway", Description = "Is endpoint a gateway")]
      public bool IsGateway { get; set; }

      [Required]
      [Display(Name = "commCapability", Description = "Communication capability")]
      public CommsDirectionCapabilities DirectionCapability { get; set; }

      [Required]
      [Display(Name = "name", Description = "Name of endpoint")]
      public string Name { get; set; }

      [Required]
      [Display(Name = "description", Description = "Short description of endpoint")]
      public string Description { get; set; }
   }
}
