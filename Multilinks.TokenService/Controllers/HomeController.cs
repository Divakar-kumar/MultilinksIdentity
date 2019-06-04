using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Multilinks.TokenService.Models;

namespace Multilinks.TokenService.Controllers
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

      public IActionResult WebConsoleHome()
      {
         return Redirect(_corsOriginsOptions.WebConsole);
      }
   }
}
