using Multilinks.ApiService.Infrastructure;
using Newtonsoft.Json;
using System;

namespace Multilinks.ApiService.Models
{
   public class UserViewModel : Resource, IEtaggable
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

      public string GetEtag()
      {
         var serialized = JsonConvert.SerializeObject(this);
         return Md5Hash.ForString(serialized);
      }
   }
}
