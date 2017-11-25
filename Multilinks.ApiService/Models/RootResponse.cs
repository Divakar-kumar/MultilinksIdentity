namespace Multilinks.ApiService.Models
{
   public class RootResponse : Resource
   {
      public Link Info { get; set; }

      public Link Users { get; set; }

      public Link Endpoints { get; set; }
   }
}
