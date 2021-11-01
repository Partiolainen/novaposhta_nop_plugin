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

        public OrderListener(
            IGenericAttributeService genericAttributeService,
            IWorkContext workContext,
            IStoreContext storeContext,
            INpOrderDataService npOrderDataService)
        {
            _genericAttributeService = genericAttributeService;
            _workContext = workContext;
            _storeContext = storeContext;
            _npOrderDataService = npOrderDataService;
        }

        public async Task HandleEventAsync(EntityInsertedEvent<Order> eventMessage)
        {
            var shippingOptions = await _genericAttributeService.GetAttributeAsync<ShippingOption>(
                await _workContext.GetCurrentCustomerAsync(),
                NopCustomerDefaults.SelectedShippingOptionAttribute,
                (await _storeContext.GetCurrentStoreAsync()).Id);

            if (shippingOptions.ShippingType != NovaPoshtaShippingType.WAREHOUSE.ToString())
            {
                return;
            }

            var orderShippingData = new NpOrderShippingData
            {
                OrderId = eventMessage.Entity.Id,
                NovaPoshtaWarehouseRef = shippingOptions.SelectedNpWarehouseRef,
                ShippingType = shippingOptions.ShippingType,
            };

            await _npOrderDataService.AddRecord(orderShippingData);
        }
    }
}