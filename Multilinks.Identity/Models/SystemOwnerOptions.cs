using System.ComponentModel.DataAnnotations;

namespace Multilinks.Identity.Models
{
   public class SystemOwnerOptions
   {
      [Required]
      [DataType(DataType.EmailAddress)]
      public string Email { get; set; }

      [Required]
      public string FirstName { get; set; }

      [Required]
      public string LastName { get; set; }

      [Required]
      public string DefaultPassword { get; set; }
   }
}
