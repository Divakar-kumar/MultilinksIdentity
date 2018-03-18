/*************************************************************************************************
 *    Class:      EndpointEntity
 * 
 *    Summary:    An endpoint is a device at either end of a communication link managed by a Multilink
 *                client.
 *             
 *    Property:   1. An endpoint will have an endpoint Id (EPID)
 *                2. An endpoint will contain the creator Id (CID) which is the Id of the registered
 *                   user who created this endpoint (note: ownership of an endpoint can be shared
 *                   multiple users but is managed by the creator of the endpoint).
 *                3. An endpoint will have a name as defined by the creator.
 *                4. An endpoint may contain a description as defined by the creator.
 * 
 *************************************************************************************************/

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multilinks.DataService.Entities
{
   public class EndpointEntity
   {
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      [Key]
      public long Id { get; set; }

      public Guid EndpointId { get; set; }

      public Guid CreatorId { get; set; }

      public string Name { get; set; }

      public string Description { get; set; }
   }
}
