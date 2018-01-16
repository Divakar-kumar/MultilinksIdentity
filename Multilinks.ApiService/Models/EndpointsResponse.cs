using Multilinks.ApiService.Infrastructure;
using Newtonsoft.Json;

namespace Multilinks.ApiService.Models
{
   public class EndpointsResponse : PagedCollection<EndpointViewModel>, IEtaggable
   {
      public Form QueryForm { get; set; }

      public Form SubmitForm { get; set; }

      public string GetEtag()
      {
         var serialized = JsonConvert.SerializeObject(this);
         return Md5Hash.ForString(serialized);
      }
   }
}
