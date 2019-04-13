using System.ComponentModel.DataAnnotations;

namespace Multilinks.ApiService.Models
{
   public class ConfirmEndpointLinkForm
   {
      [Required]
      [Display(Name = "confirmed", Description = "Link request confirmation")]
      public bool Confirmed { get; set; }
   }
}
