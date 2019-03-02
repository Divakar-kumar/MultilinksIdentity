using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multilinks.ApiService.Entities
{
   public class EndpointClientEntity
   {
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      [Key]
      public long EndpointClientId { get; set; }

      [MaxLength(128)]
      [Required]
      public string ClientId { get; set; }

      [MaxLength(128)]
      [Required]
      public string ClientType { get; set; }

      public ICollection<EndpointEntity> EndpointEntities { get; set; }
   }
}
