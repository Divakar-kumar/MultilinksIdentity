using Multilinks.ApiService.Infrastructure;
using Newtonsoft.Json;
using System;

namespace Multilinks.ApiService.Models
{
   public class EndpointViewModel : Resource, IEtaggable
   {
      public Guid EndpointId { get; set; }

      [Sortable(Default = true)]
      [Searchable]
      public string Name { get; set; }

      public string Description { get; set; }

      public string Owner { get; set; }

      public string GetEtag()
      {
         var serialized = JsonConvert.SerializeObject(this);
         return Md5Hash.ForString(serialized);
      }
   }
}
