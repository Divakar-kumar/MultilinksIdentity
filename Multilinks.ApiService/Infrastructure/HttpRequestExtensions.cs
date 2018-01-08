using Microsoft.AspNetCore.Http;

namespace Multilinks.ApiService.Infrastructure
{
   public static class HttpRequestExtensions
   {
      public static IEtagHandlerFeature GetEtagHandler(this HttpRequest request)
          => request.HttpContext.Features.Get<IEtagHandlerFeature>();
   }
}
