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

      [Required]
      public EndpointEntity SourceEndpoint { get; set; }

      [Required]
      public EndpointEntity AssociatedEndpoint { get; set; }

      [Required]
      public string Status { get; set; }
   }
}
