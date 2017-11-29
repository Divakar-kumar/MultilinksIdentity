using System.ComponentModel.DataAnnotations;

namespace Multilinks.ApiService.Models
{
   public class PagingOptions
   {
      [Range(0, 9999, ErrorMessage = "Offset must be 0 or greater and less than 9999.")]
      public int? Offset { get; set; }

      [Range(1, 100, ErrorMessage = "Limit must be greater than 0 and less than 100.")]
      public int? Limit { get; set; }
   }
}
