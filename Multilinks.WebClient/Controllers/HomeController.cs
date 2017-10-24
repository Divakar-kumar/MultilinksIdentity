using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Multilinks.WebClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace Multilinks.WebClient.Controllers
{
   [Authorize]
   public class HomeController : Controller
   {
      [HttpGet]
      [AllowAnonymous]
      public IActionResult Index()
      {
         return View();
      }

      [HttpGet]
      public IActionResult About()
      {
         ViewData["Message"] = "Your application description page.";

         return View();
      }

      [HttpGet]
      [AllowAnonymous]
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

      [HttpGet]
      public async Task<IActionResult> ApiTest()
      {
         var accessToken = await HttpContext.GetTokenAsync("access_token");

         var client = new HttpClient();
         client.SetBearerToken(accessToken);
         var content = await client.GetStringAsync("http://localhost:5001/api/users");

         ViewBag.Json = JArray.Parse(content).ToString();
         return View("json");
      }

      public IActionResult Error()
      {
         return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
      }
   }
}
