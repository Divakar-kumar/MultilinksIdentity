using System.Threading.Tasks;

namespace Multilinks.Identity.Services
{
   public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
