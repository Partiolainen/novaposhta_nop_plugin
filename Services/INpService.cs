using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Shipping;
using Nop.Plugin.Shipping.NovaPoshta.Domain;
using Nop.Plugin.Shipping.NovaPoshta.Models;
using Nop.Services.Shipping;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public interface INpService
    {
        Task<ShippingOption> GetToAddressShippingOption(GetShippingOptionRequest getShippingOptionRequest);
        Task<ShippingOption> GetToWarehouseShippingOption(GetShippingOptionRequest getShippingOptionRequest);
        Task<IList<NovaPoshtaSettlement>> GetSettlementsByAddress(Address address);
        Task<IList<NovaPoshtaWarehouse>> GetWarehousesBySettlement(NovaPoshtaSettlement settlement);
        Task<List<NovaPoshtaWarehouse>> GetWarehousesByAddress(Address address);
        Task<List<NovaPoshtaConfigurationSettingsModel.WarehouseAvailability>> GetCitiesForSendingAvailability();
        Task<NovaPoshtaWarehouse> GetWarehouseByRef(string novaPoshtaWarehouseRef);
    }
}