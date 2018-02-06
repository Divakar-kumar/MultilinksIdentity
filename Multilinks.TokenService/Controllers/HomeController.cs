using Microsoft.AspNetCore.Mvc;
using Multilinks.TokenService.Models;

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
            return View(new ErrorViewModel());
        }
    }
}
