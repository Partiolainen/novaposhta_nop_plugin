using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Shipping.NovaPoshta.Domain;
using Nop.Plugin.Shipping.NovaPoshta.Infrastructure.ApiRequest;

namespace Nop.Plugin.Shipping.NovaPoshta.Services.Factories
{
    public interface IFactoriesService
    {
        Task<OptionSeat> GetOptionSeatByProduct(Product product);
        Task<Dimensions> GetNpDimensionsByProduct(Product product);
    }
}