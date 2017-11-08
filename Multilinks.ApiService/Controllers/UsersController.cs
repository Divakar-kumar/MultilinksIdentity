using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Multilinks.ApiService.Controllers
{
   [Route("api/[controller]")]
   [ApiVersion("1.0")]
   [Authorize]
   public class UsersController : Controller
   {
      // GET api/users
      [HttpGet(Name = nameof(GetUsers))]
      [AllowAnonymous]
      public IActionResult GetUsers()
      {
         var response = new
         {
            href = Url.Link(nameof(UsersController.GetUsers), null)
         };

         return Ok(response);
      }
   }
}
