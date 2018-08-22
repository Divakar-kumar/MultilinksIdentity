using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multilinks.ApiService.Entities
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
