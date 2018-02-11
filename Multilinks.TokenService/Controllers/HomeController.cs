using Microsoft.AspNetCore.Mvc;

namespace Multilinks.TokenService.Controllers
{
   public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
