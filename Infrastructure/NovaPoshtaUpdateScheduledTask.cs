using Nop.Services.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Nop.Plugin.Shipping.NovaPoshta.Infrastructure
{
    public class NovaPoshtaUpdateScheduledTask : IScheduleTask
    {
        public Task ExecuteAsync()
        {
            
            
            return Task.CompletedTask;
        }
    }
}