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

      public Guid SourceEndpointId { get; set; }

      public Guid AssociatedEndpointId { get; set; }

      public string Status { get; set; }
   }
}
