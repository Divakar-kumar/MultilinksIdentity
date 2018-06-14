using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Multilinks.TokenService.Services
{
   // This class is used by the application to send email for account confirmation and password reset.
   // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
   public class EmailSender : IEmailSender
   {
      public async Task SendEmailAsync(string email, string subject, string htmlContent)
      {
         var apiKey = Environment.GetEnvironmentVariable("MDS_MULTILINKS_SENDGRID_API_KEY");
         var supportEmail = Environment.GetEnvironmentVariable("MDS_MULTILINKS_SUPPORT_EMAIL_ADDRESS");
         var supportName = Environment.GetEnvironmentVariable("MDS_MULTILINKS_SUPPORT_NAME");
         var client = new SendGridClient(apiKey);
         var from = new EmailAddress(supportEmail, supportName);
         var to = new EmailAddress(email);
         var plainTextContent = Regex.Replace(htmlContent, "<[^>]*>", "");
         var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
         var response = await client.SendEmailAsync(msg);
      }
   }
}
