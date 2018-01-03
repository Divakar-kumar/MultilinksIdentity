using Multilinks.ApiService.Infrastructure;
using System;

namespace Multilinks.ApiService.Models
{
   public class UserViewModel : Resource
   {
      [Sortable]
      [Searchable]
      public string Email { get; set; }

      public Guid ApplicationUserId { get; set; }

      [Sortable]
      [Searchable]
      public string Firstname { get; set; }

      [Sortable]
      [Searchable]
      public string Lastname { get; set; }

      public DateTimeOffset StartDate { get; set; }

      public DateTimeOffset EndDate { get; set; }
   }
}
