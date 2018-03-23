using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Multilinks.SpaClient.Controllers
{
   [Route("api/[Controller]")]
   //[Authorize]
   public class DevicesController : Controller
   {
      /* TODO: Only admin should be able to access this API */
      // GET api/devices/
      [HttpGet(Name = nameof(GetDevicesAsync))]
      public async Task<IActionResult> GetDevicesAsync()
      {
         /* TODO: Is this the correct way to handle SSL? */
         using(var handler = new HttpClientHandler())
         {
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls;
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            using(var client = new HttpClient(handler))
            {
               try
               {
                  /* TODO: Will need to update api address */
                  var response = await client.GetAsync($"https://localhost:44301/api/endpoints/");

                  response.EnsureSuccessStatusCode();

                  var stringResult = await response.Content.ReadAsStringAsync();
                  var devices = JsonConvert.DeserializeObject<GetDevicesResponse>(stringResult);

                  return Ok(devices);
               }
               catch(HttpRequestException httpRequestException)
               {
                  return BadRequest($"Error getting devices: {httpRequestException.Message}");
               }
            }
         }
      }

      // GET api/devices/created-by/{creatorId}
      [HttpGet("created-by/{creatorId}", Name = nameof(GetDevicesByCreatorIdAsync))]
      public async Task<IActionResult> GetDevicesByCreatorIdAsync(string creatorId)
      {
         /* TODO: Is this the correct way to handle SSL? */
         using(var handler = new HttpClientHandler())
         {
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls;
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            using(var client = new HttpClient(handler))
            {
               try
               {
                  /* TODO: Will need to update api address */
                  var response = await client.GetAsync($"https://localhost:44301/api/endpoints/created-by/{creatorId}");

                  response.EnsureSuccessStatusCode();

                  var stringResult = await response.Content.ReadAsStringAsync();
                  var devices = JsonConvert.DeserializeObject<GetDevicesResponse>(stringResult);

                  return Ok(devices);
               }
               catch(HttpRequestException httpRequestException)
               {
                  return BadRequest($"Error getting devices created by {creatorId}: {httpRequestException.Message}");
               }
            }
         }
      }
   }

   /* Dependent classes used by this controller */
   public class GetDevicesResponse
   {
      public string Offset { get; set; }

      public string Limit { get; set; }

      public string Size { get; set; }

      public IEnumerable<DeviceDetail> Value { get; set; }  /* Devices */
   }

   public class DeviceDetail
   {
      public string EndpointId { get; set; }

      public string Name { get; set; }

      public string Description { get; set; }
   }
}