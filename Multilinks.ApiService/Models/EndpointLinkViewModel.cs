using Multilinks.ApiService.Infrastructure;
using Newtonsoft.Json;
using System;

namespace Multilinks.ApiService.Models
{
   public class EndpointLinkViewModel : Resource, IEtaggable
   {
      public Guid LinkId { get; set; }

      public Guid FirstEndpointId { get; set; }

      public Guid SecondEndpointId { get; set; }

      public string Status { get; set; }

      public string GetEtag()
      {
         var serialized = JsonConvert.SerializeObject(this);
         return Md5Hash.ForString(serialized);
      }
   }
}
