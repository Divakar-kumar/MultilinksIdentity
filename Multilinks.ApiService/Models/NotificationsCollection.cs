using Multilinks.ApiService.Infrastructure;
using Newtonsoft.Json;

namespace Multilinks.ApiService.Models
{
   public class NotificationsCollection : PagedCollection<NotificationViewModel>, IEtaggable
   {
      public string GetEtag()
      {
         var serialized = JsonConvert.SerializeObject(this);
         return Md5Hash.ForString(serialized);
      }
   }
}
