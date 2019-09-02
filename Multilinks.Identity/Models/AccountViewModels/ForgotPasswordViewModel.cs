using System.ComponentModel.DataAnnotations;

namespace Multilinks.Identity.Models.AccountViewModels
{
   public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
