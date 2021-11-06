using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.SignalR;
using Nop.Core;
using Nop.Services.Logging;
using Nop.Services.Messages;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public class NotificationServiceExt : NotificationService, INotificationServiceExt
    {
        private readonly IHubContext<NotifySignalRHub> _hubContext;

        public NotificationServiceExt(
            IHttpContextAccessor httpContextAccessor, 
            ILogger logger, 
            ITempDataDictionaryFactory tempDataDictionaryFactory, 
            IWorkContext workContext,
            IHubContext<NotifySignalRHub> hubContext) 
            : base(httpContextAccessor, logger, tempDataDictionaryFactory, workContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotificationWithSignalR(NotifyType type, string message)
        {
            await _hubContext.Clients.All.SendAsync("notification", message, type);
        }

        public async Task NotificationBadProduct(string message, int id, string name)
        {
            await _hubContext.Clients.All.SendAsync("badProduct", message, id, name);
        }
    }
}