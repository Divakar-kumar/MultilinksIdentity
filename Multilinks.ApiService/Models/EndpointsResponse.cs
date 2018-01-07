namespace Multilinks.ApiService.Models
{
   public class EndpointsResponse : PagedCollection<EndpointViewModel>
   {
      public Form QueryForm { get; set; }

      public Form SubmitForm { get; set; }
   }
}
