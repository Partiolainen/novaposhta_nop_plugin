using System.Linq;
using System.Threading.Tasks;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Data;
using Nop.Plugin.Shipping.NovaPoshta.Domain;
using Nop.Plugin.Shipping.NovaPoshta.Services.Factories;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public class NpCustomerAddressService : INpCustomerAddressService
    {
        private readonly IRepository<NpCustomerAddressForOrder> _customerAddressForOrderRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly INpService _npService;

        public NpCustomerAddressService(
            IRepository<NpCustomerAddressForOrder> customerAddressForOrderRepository,
            IRepository<Order> orderRepository,
            INpService npService)
        {
            _customerAddressForOrderRepository = customerAddressForOrderRepository;
            _orderRepository = orderRepository;
            _npService = npService;
        }

        public async Task<NpCustomerAddressForOrder> InsertAddress(NpCustomerAddressForOrder npCustomerAddressForOrder)
        {
            await _customerAddressForOrderRepository.InsertAsync(npCustomerAddressForOrder);

            return npCustomerAddressForOrder;
        }

        public async Task<NpCustomerAddressForOrder> GetAddressByOrder(Order order)
        {
            var customerAddressForOrder = await _customerAddressForOrderRepository
                .GetAllAsync(query => query
                    .Where(address => address.OrderId == order.Id));

            return customerAddressForOrder.Any() ? customerAddressForOrder.First() : null;
        }

        public async Task<NpCustomerAddressForOrder> GetLastUsedAddressByCustomer(Customer customer, Address selectedAddress)
        {
            var orders = await _orderRepository
                .GetAllAsync(query => query
                    .Where(order => order.CustomerId == customer.Id));

            if (!orders.Any())
                return null;

            var ordersByDescending = orders.OrderByDescending(order => order.CreatedOnUtc).ToList();
            
            foreach (var order in ordersByDescending)
            {
                var customerAddressForOrder = await GetAddressByOrder(order);
                if (customerAddressForOrder != null && customerAddressForOrder.ZipPostalCode == selectedAddress.ZipPostalCode)
                {
                    return customerAddressForOrder;
                }
            }

            return null;
        }

        public async Task<NpCustomerAddressForOrder> TryExtractNpCustomerAddress(Address address)
        {
            var npCustomerAddressForOrder = new NpCustomerAddressForOrder();
            if (string.IsNullOrEmpty(address.ZipPostalCode))
                return npCustomerAddressForOrder;

            var novaPoshtaSettlements = await _npService.GetSettlementsByAddress(address);
            if (!novaPoshtaSettlements.Any())
                return npCustomerAddressForOrder;

            npCustomerAddressForOrder = NpCustomerAddressForOrderFactory.FromNpSettlement(novaPoshtaSettlements.First());
            npCustomerAddressForOrder.ZipPostalCode = address.ZipPostalCode;
            npCustomerAddressForOrder.Street = address.Address1;
            npCustomerAddressForOrder.PhoneNumber = address.PhoneNumber;
            npCustomerAddressForOrder.FirstName = address.FirstName;
            npCustomerAddressForOrder.LastName = address.LastName;

            return npCustomerAddressForOrder;
        }
    }
}