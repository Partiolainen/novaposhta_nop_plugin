using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Shipping;
using Nop.Core.Events;
using Nop.Services.Common;
using Nop.Services.Events;

namespace Nop.Plugin.Shipping.NovaPoshta.EventListeners
{
    public class OrderListener : IConsumer<EntityInsertedEvent<Order>>
    {
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;

        public OrderListener(
            IGenericAttributeService genericAttributeService,
            IWorkContext workContext,
            IStoreContext storeContext)
        {
            _genericAttributeService = genericAttributeService;
            _workContext = workContext;
            _storeContext = storeContext;
        }

        public async Task HandleEventAsync(EntityInsertedEvent<Order> eventMessage)
        {
            var shippingOptions = await _genericAttributeService.GetAttributeAsync<List<ShippingOption>>(
                await _workContext.GetCurrentCustomerAsync(),
                NopCustomerDefaults.OfferedShippingOptionsAttribute,
                (await _storeContext.GetCurrentStoreAsync()).Id);
            
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine($"Insert order: {eventMessage.Entity.Id}");
        }
    }
}