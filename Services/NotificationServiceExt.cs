using System.Text;
using System.Threading.Tasks;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<NotifySignalRHub> _hubContext;

        public NotificationServiceExt(
            IHttpContextAccessor httpContextAccessor, 
            ILogger logger, 
            ITempDataDictionaryFactory tempDataDictionaryFactory, 
            IWorkContext workContext,
            IHubContext<NotifySignalRHub> hubContext) 
            : base(httpContextAccessor, logger, tempDataDictionaryFactory, workContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
        }

        public async Task NotificationWithSignalR(NotifyType type, string message)
        {
            await _hubContext.Clients.All.SendAsync("notification", message, type);
        }

        public async Task NotificationBadProduct(string message, int id, string name)
        {
            await _hubContext.Clients.All.SendAsync("badProduct", message, id, name);
            if (_httpContextAccessor.HttpContext != null)
            {
                base.WarningNotification(new StringBuilder(message).Append($" (Id:{id} )").Append(name).ToString());
            }
        }
    }
}