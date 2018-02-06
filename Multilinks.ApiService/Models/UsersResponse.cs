using Multilinks.ApiService.Infrastructure;
using Newtonsoft.Json;

namespace Multilinks.ApiService.Models
{
   public class UsersResponse : PagedCollection<UserViewModel>, IEtaggable
   {
      public Form QueryForm { get; set; }

      public string GetEtag()
      {
         var serialized = JsonConvert.SerializeObject(this);
         return Md5Hash.ForString(serialized);
      }
   }
}
