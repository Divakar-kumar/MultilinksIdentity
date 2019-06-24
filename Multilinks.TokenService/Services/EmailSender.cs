using Microsoft.AspNetCore.Hosting;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Multilinks.TokenService.Services
{
   public class EmailSender : IEmailSender
   {
      private readonly IHostingEnvironment _env;

      public EmailSender(IHostingEnvironment env)
      {
         _env = env;
      }

      public async Task SendEmailAsync(string email, string subject, string htmlContent)
      {
         var apiKey = Environment.GetEnvironmentVariable("MULTILINKS_EMAIL_SERVICE_API_KEY");
         var supportEmail = Environment.GetEnvironmentVariable("MULTILINKS_EMAIL_SERVICE_EMAIL_ADDRESS");
         var supportName = Environment.GetEnvironmentVariable("MULTILINKS_EMAIL_SERVICE__NAME");

         if(_env.IsDevelopment())
         {
            /* All outgoing emails should go to a predefined email if working in dev environment. */
            email = Environment.GetEnvironmentVariable("MULTILINKS_EMAIL_SERVICE_DUMMY_EMAIL");

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
