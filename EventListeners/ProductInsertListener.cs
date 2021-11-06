using System.Drawing;
using System.Threading.Tasks;
using FluentMigrator.Runner.Generators.Postgres;
using Nop.Core.Domain.Catalog;
using Nop.Core.Events;
using Nop.Plugin.Shipping.NovaPoshta.Domain;
using Nop.Plugin.Shipping.NovaPoshta.Services;
using Nop.Plugin.Shipping.NovaPoshta.Services.Factories;
using Nop.Services.Catalog;
using Nop.Services.Events;

namespace Nop.Plugin.Shipping.NovaPoshta.EventListeners
{
    public class ProductInsertListener : IConsumer<EntityInsertedEvent<Product>>
    {
        private readonly INotificationServiceExt _notificationServiceExt;
        private readonly IFactoriesService _factoriesService;
        private readonly INpService _npService;
        private readonly NovaPoshtaSettings _novaPoshtaSettings;
        private readonly IProductService _productService;

        public ProductInsertListener(
            INotificationServiceExt notificationServiceExt,
            IFactoriesService factoriesService,
            INpService npService,
            NovaPoshtaSettings novaPoshtaSettings,
            IProductService productService)
        {
            _notificationServiceExt = notificationServiceExt;
            _factoriesService = factoriesService;
            _npService = npService;
            _novaPoshtaSettings = novaPoshtaSettings;
            _productService = productService;
        }

        public async Task HandleEventAsync(EntityInsertedEvent<Product> eventMessage)
        {
            var product = eventMessage.Entity;
            if (!product.IsShipEnabled)
                return;
            
            var dimensionsByProduct = await _factoriesService.BuildNpDimensionsByProduct(product);

            if (dimensionsByProduct.Height == 0 || dimensionsByProduct.Width == 0 || dimensionsByProduct.Length == 0)
            {
                product.Height = _novaPoshtaSettings.DefaultHeightCm;
                product.Width = _novaPoshtaSettings.DefaultWidthCm;
                product.Length = _novaPoshtaSettings.DefaultLengthCm;
                await _productService.UpdateProductAsync(product);
                
                _notificationServiceExt.WarningNotification("Для продукта не было указано один или несколько параметров размерности. " +
                                                            "Были применены размерности по умолчанию");
            }

            if (product.Weight == 0)
            {
                product.Weight = _novaPoshtaSettings.DefaultWeightKg;
                await _productService.UpdateProductAsync(product);
                
                _notificationServiceExt.WarningNotification("Для продукта не был указан вес. " +
                                                            "Были применен вес по умолчанию");
            }

            var maxDimension = new Dimensions
            {
                Height = _novaPoshtaSettings.MaxAllowedHeightCm,
                Width = _novaPoshtaSettings.MaxAllowedWidthCm,
                Length = _novaPoshtaSettings.MaxAllowedLengthCm
            };

            if (product.Weight > 30)
                _notificationServiceExt.WarningNotification("Вес товара превышает 30кг. " +
                                                            "Далеко не все отделения Новой Почты смогут принять данный товар");

            if (await _factoriesService.BuildNpDimensionsByProduct(product) <= maxDimension 
                && _novaPoshtaSettings.MaxAllowedWeightKg <= product.Weight)
                return;

            _notificationServiceExt.WarningNotification("Ни один склад Новой Почты не имеет возможности принять " +
                                                        "или отправить данный продукт. Проверьте параметры размера!");
        }
    }
}