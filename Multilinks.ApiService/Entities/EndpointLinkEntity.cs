using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multilinks.ApiService.Entities
{
   public class EndpointLinkEntity
   {
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      [Key]
      public long Id { get; set; }

      public Guid LinkId { get; set; }

      public Guid FirstEndpointId { get; set; }    // Link's source endpoint

      public Guid SecondEndpointId { get; set; }   // Link's destination endpoint

      public string Status { get; set; }
   }
}
