using System.Threading.Tasks;
using Nop.Services.Messages;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public interface INotificationServiceExt : INotificationService
    {
        Task NotificationWithSignalR(NotifyType type, string message);
        Task NotificationBadProduct(string message, int id, string name);
    }
}