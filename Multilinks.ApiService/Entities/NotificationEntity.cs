using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multilinks.ApiService.Entities
{
   public class NotificationEntity
   {
      public enum Type
      {
         None,
         LinkRequestAccepted,
         LinkRequestDenied
      }

      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      [Key]
      public long NotificationId { get; set; }

      [Required]
      public Guid Id { get; set; }

      [Required]
      public EndpointEntity RecipientEndpoint { get; set; }

      [Required]
      public Type NotificationType { get; set; }

      [MaxLength(256)]
      [Required]
      public string Message { get; set; }

      public bool Hidden { get; set; }
   }
}
