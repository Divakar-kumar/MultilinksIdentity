using System.ComponentModel.DataAnnotations;

namespace Multilinks.ApiService.Models
{
   public class UpdateNotificationForm
   {
      [Required]
      [Display(Name = "isHidden", Description = "Don't show this notification to the intended recipient.")]
      public bool Hidden { get; set; }
   }
}
