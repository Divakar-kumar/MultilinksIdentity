using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multilinks.ApiService.Entities
{
   public class EndpointLinkEntity
   {
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      [Key]
      public Guid LinkId { get; set; }

      public EndpointEntity SourceEndpoint { get; set; }

      public EndpointEntity AssociatedEndpoint { get; set; }

      [Required]
      public bool Confirmed { get; set; }
   }
}
