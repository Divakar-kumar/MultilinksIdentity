using Multilinks.TokenService.Controllers;

namespace Microsoft.AspNetCore.Mvc
{
   public static class UrlHelperExtensions
   {
      public static string RegisterConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
      {
         return urlHelper.Action(
            action: nameof(AccountController.RegisterConfirmation),
            controller: "Account",
            values: new { userId, code },
            protocol: scheme);
      }

      public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
      {
         return urlHelper.Action(
             action: nameof(AccountController.ResetPassword),
             controller: "Account",
             values: new { userId, code },
             protocol: scheme);
      }
   }
}
