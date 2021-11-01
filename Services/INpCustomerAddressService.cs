using System.Threading.Tasks;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public interface INpCustomerAddressService
    {
        public Task<NpCustomerAddressForOrder> InsertAddress(NpCustomerAddressForOrder npCustomerAddressForOrder);
        public Task<NpCustomerAddressForOrder> GetAddressByOrder(Order order);
        public Task<NpCustomerAddressForOrder> GetLastUsedAddressByCustomer(Customer customer);
        public Task<NpCustomerAddressForOrder> TryExtractNpCustomerAddress(Address address);
    }
}