using System.Text.Json;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Shipping;
using Nop.Core.Events;
using Nop.Plugin.Shipping.NovaPoshta.Domain;
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
                orderShippingData.NovaPoshtaWarehouseRef = shippingOptions.SelectedNpWarehouseRef;
            }

            if (shippingOptions.ShippingType == NovaPoshtaShippingType.ADDRESS.ToString())
            {
                var customerAddressForOrderString = await _genericAttributeService.GetAttributeAsync<string>(
                    await _workContext.GetCurrentCustomerAsync(),
                    NovaPoshtaDefaults.CustomerAddressForOrder,
                    (await _storeContext.GetCurrentStoreAsync()).Id);

                if (customerAddressForOrderString != null)
                {
                    var customerAddressForOrder = JsonSerializer.Deserialize<NpCustomerAddressForOrder>(customerAddressForOrderString);

                    if (customerAddressForOrder != null)
                    {
                        customerAddressForOrder.OrderId = eventMessage.Entity.Id;
                        await _customerAddressService.InsertAddress(customerAddressForOrder);
                        orderShippingData.NovaPoshtaCustomerAddressId = customerAddressForOrder.Id;
                    }
                }
            }

            await _npOrderDataService.AddRecord(orderShippingData);
        }
    }
}