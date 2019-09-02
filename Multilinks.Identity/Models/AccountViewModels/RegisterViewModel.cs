using System.ComponentModel.DataAnnotations;

namespace Multilinks.Identity.Models.AccountViewModels
{
   public class RegisterViewModel
   {
      [Required]
      [EmailAddress]
      [Display(Name = "Email")]
      public string Email { get; set; }
   }
}
