using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Shipping.NovaPoshta.Domain;
using Nop.Plugin.Shipping.NovaPoshta.Services.Factories;
using Nop.Services.Catalog;
using Nop.Services.Directory;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public class NpProductService : INpProductService
    {
        private readonly IFactoriesService _factoriesService;
        private readonly NovaPoshtaSettings _novaPoshtaSettings;
        private readonly IProductService _productService;
        private readonly INotificationServiceExt _notificationServiceExt;
        private readonly IMeasureService _measureService;

        public NpProductService(
            IFactoriesService factoriesService,
            NovaPoshtaSettings novaPoshtaSettings,
            IProductService productService,
            INotificationServiceExt notificationServiceExt,
            IMeasureService measureService)
        {
            _factoriesService = factoriesService;
            _novaPoshtaSettings = novaPoshtaSettings;
            _productService = productService;
            _notificationServiceExt = notificationServiceExt;
            _measureService = measureService;
        }

        public async Task<bool> CheckDimensionValuesForZeros(Product product)
        {
            if (product.Length != 0 && product.Width != 0 && product.Height != 0) return true;
            
            await _notificationServiceExt.NotificationBadProduct(
                "Для товара не была задана размерность. ", product.Id, product.Name);
                
            return false;

        }
        
        public async Task<Product> SetDefaultDimensions(Product product)
        {
            var centimetresDimension = await _measureService.GetMeasureDimensionByIdAsync(_novaPoshtaSettings.CentimetresMeasureDimensionId);

            product.Length = await _measureService.ConvertToPrimaryMeasureDimensionAsync(_novaPoshtaSettings.DefaultLengthCm, centimetresDimension);
            product.Height = await _measureService.ConvertToPrimaryMeasureDimensionAsync(_novaPoshtaSettings.DefaultHeightCm, centimetresDimension);
            product.Width = await _measureService.ConvertToPrimaryMeasureDimensionAsync(_novaPoshtaSettings.DefaultWidthCm, centimetresDimension);

            await _productService.UpdateProductAsync(product);
            
            await _notificationServiceExt.NotificationBadProduct(
                "Установлены значения размерности по умолчанию. ", product.Id, product.Name);
            
            return product;
        }

        public async Task<bool> CheckWeightAndValueForZero(Product product)
        {
            if (product.Weight != 0) return true;

            await _notificationServiceExt.NotificationBadProduct(
                "Для товара не был задан вес. ", product.Id, product.Name);
            
            return false;
        }

        public async Task<Product> SetDefaultWeight(Product product)
        {
            var kilogramsDimension = await _measureService.GetMeasureWeightByIdAsync(_novaPoshtaSettings.KilogramsMeasureDimensionId);

            product.Weight = await _measureService.ConvertToPrimaryMeasureWeightAsync(_novaPoshtaSettings.DefaultWeightKg, kilogramsDimension);
                
            await _productService.UpdateProductAsync(product);
                
            await _notificationServiceExt.NotificationBadProduct(
                "Установлно значение веса по умолчанию. ", product.Id, product.Name);

            return product;
        }

        public async Task<bool> MatchDimensionsToWarehouse(Product product, NovaPoshtaWarehouse warehouse)
        {
            var productDimensions = await _factoriesService.GetNpDimensionsByProduct(product);

            return warehouse.ReceivingLimitationsOnDimensions >= productDimensions;
        }

        public async Task<bool> MatchDimensionsToMaxAllowed(Product product)
        {
            var productDimensions = await _factoriesService.GetNpDimensionsByProduct(product);

            return productDimensions <= _novaPoshtaSettings.GetMaxAllowedDimensions();
        }

        public bool MatchWeightToWarehouse(Product product, NovaPoshtaWarehouse warehouse)
        {
            if (int.Parse(warehouse.PlaceMaxWeightAllowed) > 0)
            {
                return product.Weight <= decimal.Parse(warehouse.PlaceMaxWeightAllowed);
            }

            if (int.Parse(warehouse.TotalMaxWeightAllowed) > 0)
            {
                return product.Weight <= decimal.Parse(warehouse.TotalMaxWeightAllowed);
            }

            return true;
        }

        public async Task<bool> MatchWeightToMaxAllowed(Product product)
        {
            var kilogramsDimension = await _measureService.GetMeasureWeightByIdAsync(_novaPoshtaSettings.KilogramsMeasureDimensionId);

            var maxAllowedWeightInPrimaryMeasureWeight = await _measureService
                .ConvertToPrimaryMeasureWeightAsync(_novaPoshtaSettings.MaxAllowedWeightKg, kilogramsDimension);
            
            return product.Weight <= maxAllowedWeightInPrimaryMeasureWeight;
        }

        public bool MatchDeclaredPriceToWarehouse(Product product, NovaPoshtaWarehouse warehouse)
        {
            if (int.Parse(warehouse.MaxDeclaredCost) > 0)
            {
                return product.Price <= decimal.Parse(warehouse.MaxDeclaredCost);
            }

            return true;
        }

        public async Task CheckAllShippingMeasures(Product product, bool setToDefault = true)
        {
            if (!await CheckDimensionValuesForZeros(product) && setToDefault)
            {
                await SetDefaultDimensions(product);
            }

            if (!await CheckWeightAndValueForZero(product) && setToDefault)
            {
                await SetDefaultWeight(product);
            }

            if (product.Weight > 30)
                _notificationServiceExt.WarningNotification("Вес товара превышает 30кг. " +
                                                            "Далеко не все отделения Новой Почты смогут принять данный товар");

            if (await MatchDimensionsToMaxAllowed(product) && await MatchWeightToMaxAllowed(product)) return;

            _notificationServiceExt.WarningNotification("Ни один склад Новой Почты не имеет возможности принять " +
                                                        "или отправить данный продукт. Проверьте параметры размера!");
        }
    }
}