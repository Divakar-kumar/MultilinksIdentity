using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Multilinks.ApiService.Controllers
{
   [Route("/")]
   [ApiVersion("1.0")]
   [Authorize]
   public class RootController : Controller
   {
      [HttpGet(Name = nameof(GetRoot))]
      [AllowAnonymous]
      public IActionResult GetRoot()
      {
         var response = new
         {
            href = Url.Link(nameof(GetRoot), null),
            users = new { href = Url.Link(nameof(UsersController.GetUsers), null) },
            info = new { href = Url.Link(nameof(InfoController.GetInfo), null) }
         };

         return Ok(response);
      }
   }
}
