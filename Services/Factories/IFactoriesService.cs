using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Shipping.NovaPoshta.Domain;
using Nop.Plugin.Shipping.NovaPoshta.Infrastructure.ApiRequest;

namespace Nop.Plugin.Shipping.NovaPoshta.Services.Factories
{
    public interface IFactoriesService
    {
        Task<OptionSeat> BuildOptionSeatByProduct(Product product);
        Task<Dimensions> BuildNpDimensionsByProduct(Product product);
    }
}