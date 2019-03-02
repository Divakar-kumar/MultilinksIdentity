using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multilinks.ApiService.Entities
{
   public class EndpointOwnerEntity
   {
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      [Key]
      public long EndpointOwnerId { get; set; }

      [Required]
      public Guid IdentityId { get; set; }

      [MaxLength(128)]
      [Required]
      public string OwnerName { get; set; }

      public ICollection<EndpointEntity> EndpointEntities { get; set; }
   }
}
