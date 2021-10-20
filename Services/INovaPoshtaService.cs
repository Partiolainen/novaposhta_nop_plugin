using System.Threading.Tasks;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Shipping;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public interface INovaPoshtaService
    {
        
        
        Task<ShippingOption> GetToAddressShippingOption(Address shippingAddress);
        
        Task<ShippingOption> GetToWarehouseShippingOption(Address shippingAddress);
    }
}