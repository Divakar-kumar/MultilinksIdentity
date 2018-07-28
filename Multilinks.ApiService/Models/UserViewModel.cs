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

      public string GetEtag()
      {
         var serialized = JsonConvert.SerializeObject(this);
         return Md5Hash.ForString(serialized);
      }
   }
}
