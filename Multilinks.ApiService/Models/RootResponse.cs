using Multilinks.ApiService.Infrastructure;
using Newtonsoft.Json;

namespace Multilinks.ApiService.Models
{
   public class RootResponse : Resource, IEtaggable
   {
      public Link Endpoints { get; set; }

      public string GetEtag()
      {
         var serialized = JsonConvert.SerializeObject(this);
         return Md5Hash.ForString(serialized);
      }
   }
}
