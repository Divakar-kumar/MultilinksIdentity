using System.ComponentModel.DataAnnotations;

namespace Multilinks.TokenService.Models.AccountViewModels
{
   public class RegisterViewModel
   {
      [Required]
      [EmailAddress]
      [Display(Name = "Email")]
      public string Email { get; set; }
   }
}
