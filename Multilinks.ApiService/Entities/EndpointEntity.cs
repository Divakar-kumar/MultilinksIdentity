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

      public string ClientId { get; set; }

      public string ClientType { get; set; }

      public string Name { get; set; }

      public string Description { get; set; }

      public string CreatorName { get; set; }
   }
}
