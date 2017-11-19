using Newtonsoft.Json;

namespace Multilinks.ApiService.Models
{
   public abstract class Resource
   {
      [JsonProperty(Order = -2)]
      public string Href { get; set; }
   }
}
