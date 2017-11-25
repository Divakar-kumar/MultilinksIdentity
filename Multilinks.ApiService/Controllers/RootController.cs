using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multilinks.ApiService.Models;

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
         var response = new RootResponse
         {
            Self = Link.To(nameof(GetRoot)),
            Info = Link.To(nameof(InfoController.GetInfo)),
            Users = Link.To(nameof(UsersController.GetUsers)),
            Endpoints = Link.To(nameof(EndpointsController.GetEndpoints))
         };

         return Ok(response);
      }
   }
}
