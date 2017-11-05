using Microsoft.AspNetCore.Mvc;

namespace Multilinks.ApiService.Controllers
{
   [Route("/")]
   public class RootController : Controller
   {
      [HttpGet(Name = nameof(GetRoot))]
      public IActionResult GetRoot()
      {
         var response = new
         {
            href = Url.Link(nameof(GetRoot), null)
         };

         return Ok(response);
      }
   }
}
