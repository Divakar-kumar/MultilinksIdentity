using Multilinks.ApiService.Infrastructure;
using System;

namespace Multilinks.ApiService.Models
{
   public class UserViewModel : Resource
   {
      [Sortable]
      [SearchableBoolean]
      public string Email { get; }

      public Guid ApplicationUserId { get; }

      [Sortable]
      [SearchableBoolean]
      public string Firstname { get; }

      [Sortable]
      [SearchableBoolean]
      public string Lastname { get; }

      public DateTimeOffset StartDate { get; }

      public DateTimeOffset EndDate { get; }
   }
}
