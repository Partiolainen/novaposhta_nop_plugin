using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public interface INovaPoshtaApiService
    {
        Task<List<NovaPoshtaAddress>> GetAddressesByCityName(string cityName);
        Task<List<NovaPoshtaSettlement>> GetAllSettlements();
        Task<List<NovaPoshtaSettlement>> GetSettlementsByRef(string @ref);
        Task<List<NovaPoshtaWarehouse>> GetAllWarehouses();
        Task<List<NovaPoshtaWarehouse>> GetWarehousesByCityRef(string cityRef);
    }
}