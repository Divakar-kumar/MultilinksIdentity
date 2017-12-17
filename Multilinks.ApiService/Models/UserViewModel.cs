using Multilinks.ApiService.Infrastructure;
using System;

namespace Multilinks.ApiService.Models
{
   public class UserViewModel : Resource
   {
      [Sortable]
      [SearchableBoolean]
      public string Email { get; set; }

      public Guid ApplicationUserId { get; set; }

      [Sortable]
      [SearchableBoolean]
      public string Firstname { get; set; }

      [Sortable]
      [SearchableBoolean]
      public string Lastname { get; set; }

      public DateTimeOffset StartDate { get; set; }

      public DateTimeOffset EndDate { get; set; }
   }
}
