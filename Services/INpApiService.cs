using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public interface INpApiService
    {
        Task<List<NovaPoshtaAddress>> GetAddressesByCityName(string cityName);
        Task<List<NovaPoshtaSettlement>> GetAllSettlements();
        Task<List<NovaPoshtaSettlement>> GetSettlementsByRef(string @ref);
        Task<List<NovaPoshtaWarehouse>> GetAllWarehouses();
        Task<List<NovaPoshtaWarehouse>> GetWarehousesByCityRef(string cityRef);
        Task<List<NovaPoshtaArea>> GetAllAreas();
        Task<List<NovaPoshtaDocumentPrice>> GetDeliveryPrice(NovaPoshtaSettlement sender,
            NovaPoshtaSettlement recipient, Product product, bool toWarehouse = true);
    }
}