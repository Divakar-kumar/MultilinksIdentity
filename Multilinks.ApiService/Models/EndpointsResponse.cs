namespace Multilinks.ApiService.Models
{
   public class EndpointsResponse : PagedCollection<EndpointViewModel>
   {
      public Form QueryForm { get; set; }
   }
}
