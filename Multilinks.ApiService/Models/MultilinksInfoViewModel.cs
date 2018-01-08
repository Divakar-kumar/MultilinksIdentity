using Multilinks.ApiService.Infrastructure;
using Newtonsoft.Json;

namespace Multilinks.ApiService.Models
{
   public class MultilinksInfoViewModel : Resource, IEtaggable
   {
      public string Title { get; set; }
      public string Tagline { get; set; }
      public string Email { get; set; }
      public string Website { get; set; }
      public Address Location { get; set; }

      public string GetEtag()
      {
         var serialized = JsonConvert.SerializeObject(this);
         return Md5Hash.ForString(serialized);
      }
   }

   public class Address
   {
      public string City { get; set; }
      public string Country { get; set; }
   }
}
