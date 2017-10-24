using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Multilinks.WebClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace Multilinks.WebClient.Controllers
{
   public class HomeController : Controller
   {
      [HttpGet]
      public IActionResult Index()
      {
         return View();
      }

      [Authorize]
      [HttpGet]
      public IActionResult About()
      {
         ViewData["Message"] = "Your application description page.";

         return View();
      }

      [HttpGet]
      public IActionResult Contact()
      {
         ViewData["Message"] = "Your contact page.";

         return View();
      }

      [Authorize]
      [HttpPost]
      public IActionResult Logout()
      {
         return SignOut(new AuthenticationProperties
         {
            RedirectUri = "/Home/Index"
         }, "Cookies", "oidc");
      }

      public IActionResult Error()
      {
         return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
      }
   }
}
