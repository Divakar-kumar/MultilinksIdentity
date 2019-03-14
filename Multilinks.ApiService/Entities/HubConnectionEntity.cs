using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multilinks.ApiService.Entities
{
   public class HubConnectionEntity
   {
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      [Key]
      public long Id { get; set; }

      [Required]
      public string ConnectionId { get; set; }

      [Required]
      public bool Connected { get; set; }
   }
}
