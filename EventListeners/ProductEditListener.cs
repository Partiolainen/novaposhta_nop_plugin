using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Core.Events;
using Nop.Plugin.Shipping.NovaPoshta.Services;
using Nop.Services.Events;

namespace Nop.Plugin.Shipping.NovaPoshta.EventListeners
{
    public class ProductEditListener : IConsumer<EntityUpdatedEvent<Product>>
    {
        private readonly INpProductService _npProductService;

        public ProductEditListener(INpProductService npProductService)
        {
            _npProductService = npProductService;
        }
        
        public async Task HandleEventAsync(EntityUpdatedEvent<Product> eventMessage)
        {
            var product = eventMessage.Entity;
            
            if (!product.IsShipEnabled) return;

            await _npProductService.CheckAllShippingMeasures(product);
        }
    }
}