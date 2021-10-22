using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Shipping;
using Nop.Plugin.Shipping.NovaPoshta.Domain;
using Nop.Plugin.Shipping.NovaPoshta.Models;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public interface INovaPoshtaService
    {
        Task<ShippingOption> GetToAddressShippingOption(Address shippingAddress);
        Task<ShippingOption> GetToWarehouseShippingOption(Address shippingAddress);
        Task<IList<NovaPoshtaSettlement>> GetSettlementsByAddress(Address address);
        Task<IList<NovaPoshtaWarehouse>> GetWarehousesBySettlement(NovaPoshtaSettlement settlement);
        Task<List<NovaPoshtaWarehouse>> GetWarehousesByAddress(Address address);
        Task<List<NovaPoshtaConfigurationSettingsModel.WarehouseAvailability>> GetCitiesForSendingAvailability();
    }
}