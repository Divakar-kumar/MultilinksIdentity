/*************************************************************************************************
 *    Class:      EndpointEntity
 * 
 *    Summary:    An endpoint is a device at either end of a communication link and can either
 *                be TCP/IP capable devices or non-TCP/IP capable devices.
 *                An endpoint that is TCP/IP capable will be driven by a Multilink Client
 *                installed on that endpoint. These endpoints will be referred to as cloud-connected
 *                endpoints.
 *                An endpoint that is not TCP/IP capable (e.g. an IR receiver, LMR, OBD) will need to
 *                be managed by a TCP/IP capable device which is driven by a Multilink Gateway. These
 *                endpoints will be referred to as non cloud-connected endpoints.
 *             
 *    Property:   1. An endpoint will have an endpoint Id (EPID) and a service area Id (SAID)
 *                2. The EPID of an endpoint is unique within the service area of the endpoint.
 *                3. An endpoint will contain the creator Id (CID) which is the Id of the registered
 *                   user who created this endpoint (note: ownership of an endpoint can be shared
 *                   multiple users but is managed by the creator of the endpoint).
 *                4. An endpoint will need to indicate if it is a cloud-connected endpoint or not.
 *                5. An endpoint will need to indicate if it is a Multilink Gateway.
 *                6. An endpoint will need to indicate if it can receive only, transmit only or both.
 *                7. An endpoint will have a label as defined by the creator.
 *                8. An endpoint may contain a description as defined by the creator.
 * 
 *************************************************************************************************/

using System;
using System.ComponentModel.DataAnnotations;

namespace Multilinks.ApiService.Models
{
   public class EndpointEntity
   {
      public enum CommsDirectionCapabilities
      {
         receiveOnly,   /* E.g. IR Receiver */
         transmitOnly,     /* E.g. An actuator */
         transmitAndReceive
      };

      [Key]
      public Guid EndpointId { get; set; }

      public Guid ServiceAreaId { get; set; }

      public Guid CreatorId { get; set; }

      public bool IsCloudConnected { get; set; }

      public bool IsGateway { get; set; }

      public CommsDirectionCapabilities DirectionCapability { get; set; }

      public string Name { get; set; }

      public string Description { get; set; }
   }
}
