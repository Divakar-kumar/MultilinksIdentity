using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multilinks.ApiService.Entities
{
   public class EndpointEntity
   {
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      [Key]
      public Guid EndpointId { get; set; }

      [MaxLength(128)]
      [Required]
      public string Name { get; set; }

      [MaxLength(512)]
      public string Description { get; set; }

      [Required]
      public EndpointClientEntity Client { get; set; }

      [Required]
      public EndpointOwnerEntity Owner { get; set; }

      [Required]
      public HubConnectionEntity HubConnection { get; set; }

      public ICollection<NotificationEntity> NotificationEntities { get; set; }
   }
}
