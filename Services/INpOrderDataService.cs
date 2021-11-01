using System.Threading.Tasks;
using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public interface INpOrderDataService
    {
        Task<NpOrderShippingData> AddRecord(NpOrderShippingData record);
        Task<NpOrderShippingData> GetByOrderId(int orderId);
    }
}