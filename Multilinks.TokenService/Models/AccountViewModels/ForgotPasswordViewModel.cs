using System.ComponentModel.DataAnnotations;

namespace Multilinks.TokenService.Models.AccountViewModels
{
   public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
