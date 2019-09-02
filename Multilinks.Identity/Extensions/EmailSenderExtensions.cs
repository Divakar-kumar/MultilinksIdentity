using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Multilinks.Identity.Services
{
   public static class EmailSenderExtensions
   {
      public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
      {
         return emailSender.SendEmailAsync(email, "Confirm your email",
             $"Please follow this <a href='{HtmlEncoder.Default.Encode(link)}'>link</a> to complete your account registration.");
      }

      public static Task SendEmailForgotPasswordAsync(this IEmailSender emailSender, string email, string link)
      {
         return emailSender.SendEmailAsync(email, "Reset Password",
             $"Please follow this <a href='{HtmlEncoder.Default.Encode(link)}'>link</a> to reset your password.");
      }
   }
}
