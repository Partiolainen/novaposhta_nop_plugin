using System.Globalization;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Shipping.NovaPoshta.Domain;
using Nop.Plugin.Shipping.NovaPoshta.Infrastructure.ApiRequest;
using Nop.Services.Directory;

namespace Nop.Plugin.Shipping.NovaPoshta.Services.Factories
{
    public class FactoriesService : IFactoriesService
    {
        private readonly IMeasureService _measureService;
        private readonly NovaPoshtaSettings _novaPoshtaSettings;

        public FactoriesService(
            IMeasureService measureService,
            NovaPoshtaSettings novaPoshtaSettings)
        {
            _measureService = measureService;
            _novaPoshtaSettings = novaPoshtaSettings;
        }

        public async Task<OptionSeat> GetOptionSeatByProduct(Product product)
        {
            var centimetresMeasureDimension = await _measureService
                .GetMeasureDimensionByIdAsync(_novaPoshtaSettings.CentimetresMeasureDimensionId);

            var kilogramsMeasureWeight = await _measureService
                .GetMeasureWeightByIdAsync(_novaPoshtaSettings.KilogramsMeasureDimensionId);
            
            var optionSeat = new OptionSeat
            {
                volumetricHeight =
                    (await _measureService.ConvertFromPrimaryMeasureDimensionAsync(product.Height, centimetresMeasureDimension))
                    .ToString(CultureInfo.InvariantCulture),
                volumetricLength = 
                    (await _measureService.ConvertFromPrimaryMeasureDimensionAsync(product.Length, centimetresMeasureDimension))
                    .ToString(CultureInfo.InvariantCulture),
                volumetricWidth = 
                    (await _measureService.ConvertFromPrimaryMeasureDimensionAsync(product.Width, centimetresMeasureDimension))
                    .ToString(CultureInfo.InvariantCulture),
                weight = 
                    (await _measureService.ConvertFromPrimaryMeasureWeightAsync(product.Weight, kilogramsMeasureWeight))
                    .ToString(CultureInfo.InvariantCulture)
            };

            return optionSeat;
        }

        public async Task<Dimensions> GetNpDimensionsByProduct(Product product)
        {
            var optionSeatByProduct = await GetOptionSeatByProduct(product);

            var dimensions = new Dimensions
            {
                Height = int.Parse(optionSeatByProduct.volumetricHeight.Split('.')[0]),
                Length = int.Parse(optionSeatByProduct.volumetricLength.Split('.')[0]),
                Width = int.Parse(optionSeatByProduct.volumetricWidth.Split('.')[0]),
            };

            return dimensions;
        }
    }
}