﻿using System.ComponentModel.DataAnnotations;

namespace Multilinks.Identity.Models.AccountViewModels
{
   public class ResetPasswordViewModel
   {
      [Required]
      [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
      [DataType(DataType.Password)]
      public string Password { get; set; }

      [DataType(DataType.Password)]
      [Display(Name = "Confirm password")]
      [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
      public string ConfirmPassword { get; set; }

      public string Code { get; set; }
      public string UserId { get; set; }

   }
}
