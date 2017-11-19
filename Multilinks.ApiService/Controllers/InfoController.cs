using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Multilinks.ApiService.Models;

namespace Multilinks.ApiService.Controllers
{
   [Route("api/[controller]")]
   [ApiVersion("1.0")]
   [Authorize]
   public class InfoController : Controller
   {
      private readonly MultilinksInfo _multilinksInfo;

      public InfoController(IOptions<MultilinksInfo> multilinksInfo)
      {
         _multilinksInfo = multilinksInfo.Value;
      }

      // GET api/info
      [HttpGet(Name = nameof(GetInfo))]
      [AllowAnonymous]
      public IActionResult GetInfo()
      {
         _multilinksInfo.Href = Url.Link(nameof(InfoController.GetInfo), null);

         return Ok(_multilinksInfo);
      }
   }
}
