using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Shipping;
using Nop.Core.Events;
using Nop.Plugin.Shipping.NovaPoshta.Domain;
using Nop.Plugin.Shipping.NovaPoshta.Infrastructure.GenericAttributes;
using Nop.Plugin.Shipping.NovaPoshta.Services;
using Nop.Services.Common;
using Nop.Services.Events;

namespace Nop.Plugin.Shipping.NovaPoshta.EventListeners
{
    public class OrderListener : IConsumer<EntityInsertedEvent<Order>>
    {
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly INpOrderDataService _npOrderDataService;
        private readonly INpCustomerAddressService _customerAddressService;

        public OrderListener(
            IGenericAttributeService genericAttributeService,
            IWorkContext workContext,
            IStoreContext storeContext,
            INpOrderDataService npOrderDataService,
            INpCustomerAddressService customerAddressService)
        {
            _genericAttributeService = genericAttributeService;
            _workContext = workContext;
            _storeContext = storeContext;
            _npOrderDataService = npOrderDataService;
            _customerAddressService = customerAddressService;
        }

        public async Task HandleEventAsync(EntityInsertedEvent<Order> eventMessage)
        {
            var shippingOptions = await _genericAttributeService.GetAttributeAsync<ShippingOption>(
                await _workContext.GetCurrentCustomerAsync(),
                NopCustomerDefaults.SelectedShippingOptionAttribute,
                (await _storeContext.GetCurrentStoreAsync()).Id);

            var orderShippingData = new NpOrderShippingData
            {
                OrderId = eventMessage.Entity.Id,
                ShippingType = shippingOptions.ShippingType,
            };
            
            if (shippingOptions.ShippingType == NovaPoshtaShippingType.WAREHOUSE.ToString())
            {
                var toWarehouseCustomerMainInfo = await _genericAttributeService.GetAttributeAsync<ToWarehouseCustomerMainInfo>(
                    await _workContext.GetCurrentCustomerAsync(),
                    NovaPoshtaDefaults.CustomerMainInfoForOrder,
                    (await _storeContext.GetCurrentStoreAsync()).Id);
                
                orderShippingData.NovaPoshtaWarehouseRef = toWarehouseCustomerMainInfo.WarehouseRef;
                orderShippingData.FirstName = toWarehouseCustomerMainInfo.FirstName;
                orderShippingData.LastName = toWarehouseCustomerMainInfo.LastName;
                orderShippingData.PhoneNumber = toWarehouseCustomerMainInfo.PhoneNumber;
            }

            if (shippingOptions.ShippingType == NovaPoshtaShippingType.ADDRESS.ToString())
            {
                var customerAddressForOrder = await _genericAttributeService.GetAttributeAsync<NpCustomerAddressForOrder>(
                    await _workContext.GetCurrentCustomerAsync(),
                    NovaPoshtaDefaults.CustomerAddressForOrder,
                    (await _storeContext.GetCurrentStoreAsync()).Id);

                if (customerAddressForOrder != null)
                {
                    customerAddressForOrder.OrderId = eventMessage.Entity.Id;
                    await _customerAddressService.InsertAddress(customerAddressForOrder);
                    orderShippingData.NovaPoshtaCustomerAddressId = customerAddressForOrder.Id;
                }
            }

            await _npOrderDataService.AddRecord(orderShippingData);
        }
    }
}