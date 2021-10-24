using System.Threading.Tasks;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public interface INpScheduleTasksService
    {
        public Task UpdateDatabase(bool force = false);
    }
}