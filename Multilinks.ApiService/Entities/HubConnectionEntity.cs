using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multilinks.ApiService.Entities
{
   public class HubConnectionEntity
   {
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      [Key]
      public long Id { get; set; }
      public string ConnectionId { get; set; }
      public Guid EndpointId { get; set; }
      public string AccessLevel { get; set; }
      public string Token { get; set; }
   }
}
