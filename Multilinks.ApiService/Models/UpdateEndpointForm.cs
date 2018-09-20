using System.ComponentModel.DataAnnotations;

namespace Multilinks.ApiService.Models
{
   public class UpdateEndpointForm
   {
      [Required]
      [Display(Name = "description", Description = "Short description of endpoint")]
      public string Description { get; set; }
   }
}
