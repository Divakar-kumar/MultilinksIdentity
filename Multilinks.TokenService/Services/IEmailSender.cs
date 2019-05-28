using System.Threading.Tasks;

namespace Multilinks.TokenService.Services
{
   public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
