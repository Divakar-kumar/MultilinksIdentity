using Microsoft.AspNetCore.Hosting;
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
      private readonly IHostingEnvironment _env;

      public EmailSender(IHostingEnvironment env)
      {
         _env = env;
      }

      public async Task SendEmailAsync(string email, string subject, string htmlContent)
      {
         var apiKey = Environment.GetEnvironmentVariable("MDS_MULTILINKS_SENDGRID_API_KEY");
         var supportEmail = Environment.GetEnvironmentVariable("MDS_MULTILINKS_SUPPORT_EMAIL_ADDRESS");
         var supportName = Environment.GetEnvironmentVariable("MDS_MULTILINKS_SUPPORT_NAME");

         if(_env.IsDevelopment())
         {
            /* All outgoing emails should go to a predefined email if working in dev environment. */
            email = Environment.GetEnvironmentVariable("MDS_MULTILINKS_SENDGRID_DUMMY_EMAIL");

            if (email == null)
            {
               email = "";
            }
         }

         if(apiKey == null || supportEmail == null || supportName == null)
         {
            throw new ApplicationException("Email service info not found.");
         }

         var client = new SendGridClient(apiKey);
         var from = new EmailAddress(supportEmail, supportName);
         var to = new EmailAddress(email);
         var plainTextContent = Regex.Replace(htmlContent, "<[^>]*>", "");
         var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
         var response = await client.SendEmailAsync(msg);
      }
   }
}
