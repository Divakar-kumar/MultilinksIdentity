using Multilinks.ApiService.Entities;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Hubs.Interfaces
{
   public interface IMainHub
   {
      Task LinkRequestReceived(string linkId, string sourceDeviceName, string sourceDeviceOwnerName);

      Task LinkConfirmationReceived(string linkId, string associatedDeviceName, string associatedDeviceOwnerName, bool isActive);

      Task NotificationReceived(string id, NotificationEntity.Type notificationType, string message, bool hidden);
   }
}
