using System.Threading.Tasks;
using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public interface INovaPoshtaWarehouseForOrderService
    {
        Task<NovaPoshtaWarehouseForOrder> AddRecord(NovaPoshtaWarehouseForOrder record);
        Task<NovaPoshtaWarehouseForOrder> GetByOrderId(int orderId);
    }
}