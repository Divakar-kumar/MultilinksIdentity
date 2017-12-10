using System.ComponentModel.DataAnnotations;

namespace Multilinks.TokenService.Models.AccountViewModels
{
   public class RegisterAdminViewModel
   {
      [Required]
      [Display(Name = "First Name")]
      public string Firstname { get; set; }

      [Required]
      [Display(Name = "Last Name")]
      public string Lastname { get; set; }

      [Required]
      [EmailAddress]
      [Display(Name = "Email")]
      public string Email { get; set; }

      [Required]
      [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
      [DataType(DataType.Password)]
      [Display(Name = "Password")]
      public string Password { get; set; }

      [DataType(DataType.Password)]
      [Display(Name = "Confirm Password")]
      [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
      public string ConfirmPassword { get; set; }
   }
}
