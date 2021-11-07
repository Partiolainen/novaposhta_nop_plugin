using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public interface INpProductService
    {
        Task<bool> CheckDimensionValuesForZeros(Product product);
        Task<Product> SetDefaultDimensions(Product product);
        Task<bool> CheckWeightAndValueForZero(Product product);
        Task<Product> SetDefaultWeight(Product product);
        Task<bool> MatchDimensionsToWarehouse(Product product, NovaPoshtaWarehouse warehouse);
        Task<bool> MatchDimensionsToMaxAllowed(Product product);
        bool MatchWeightToWarehouse(Product product, NovaPoshtaWarehouse warehouse);
        Task<bool> MatchWeightToMaxAllowed(Product product);
        bool MatchDeclaredPriceToWarehouse(Product product, NovaPoshtaWarehouse warehouse);
        Task CheckAllShippingMeasures(Product product, bool setToDefault = true);
    }
}