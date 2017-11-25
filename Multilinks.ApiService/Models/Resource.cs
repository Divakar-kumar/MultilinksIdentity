using Newtonsoft.Json;

namespace Multilinks.ApiService.Models
{
   public abstract class Resource : Link
   {
      [JsonIgnore]
      public Link Self { get; set; }
   }
}
