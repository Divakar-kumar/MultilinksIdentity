using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Multilinks.Identity.Models;

namespace Multilinks.Identity.Controllers
{
   public class HomeController : Controller
   {
      private readonly CorsOriginsOptions _corsOriginsOptions;

      public HomeController(IOptions<CorsOriginsOptions> corsOriginsOptions)
      {
         _corsOriginsOptions = corsOriginsOptions.Value;
      }

      public IActionResult Index()
      {
         return View();
      }

      public IActionResult Error()
      {
         return View();
      }

      public IActionResult PortalHome()
      {
         return Redirect(_corsOriginsOptions.Portal);
      }
   }
}
