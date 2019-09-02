using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Multilinks.Identity.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Multilinks.Identity.Services
{
   public class EmailSender : IEmailSender
   {
      private readonly IHostingEnvironment _env;
      private readonly EmailServiceOptions _emailServiceOptions;

      public EmailSender(IHostingEnvironment env, IOptions<EmailServiceOptions> emailServiceOptions)
      {
         _env = env;
         _emailServiceOptions = emailServiceOptions.Value;
      }

      public async Task SendEmailAsync(string email, string subject, string htmlContent)
      {
         var apiKey = _emailServiceOptions.ApiKey;
         var supportEmail = _emailServiceOptions.Email;
         var supportName = _emailServiceOptions.Name;

         if (_env.IsDevelopment())
         {
            /* All outgoing emails should go to supportEmail if working in dev environment. */
            email = supportEmail;

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
