using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Shipping;
using Nop.Data;
using Nop.Plugin.Shipping.NovaPoshta.Data;
using Nop.Plugin.Shipping.NovaPoshta.Domain;
using Nop.Plugin.Shipping.NovaPoshta.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Tasks;
using NUglify.Helpers;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public class NovaPoshtaService : INovaPoshtaService
    {
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly INovaPoshtaRepository<NovaPoshtaSettlement> _settlementsRepository;
        private readonly INovaPoshtaRepository<NovaPoshtaWarehouse> _warehousesRepository;
        private readonly IRepository<Warehouse> _nopWarehousesRepository;
        private readonly IRepository<Address> _nopAddressesRepository;

        public NovaPoshtaService(
            ISettingService settingService,
            ILocalizationService localizationService,
            INovaPoshtaRepository<NovaPoshtaSettlement> settlementsRepository,
            INovaPoshtaRepository<NovaPoshtaWarehouse> warehousesRepository,
            IRepository<Warehouse> nopWarehousesRepository,
            IRepository<Address> nopAddressesRepository)
        {
            _settingService = settingService;
            _localizationService = localizationService;
            _settlementsRepository = settlementsRepository;
            _warehousesRepository = warehousesRepository;
            _nopWarehousesRepository = nopWarehousesRepository;
            _nopAddressesRepository = nopAddressesRepository;
        }

        public async Task<decimal> GetRateToWarehouse(Address shippingAddress)
        {
            var novaPoshtaSettings = await GetSettings();


            return 11;
        }

        public async Task<decimal> GetRateToAddress(Address shippingAddress)
        {
            var novaPoshtaSettings = await GetSettings();

            return 21;
        }

        public async Task<int> GetTransitToWarehouse(Address shippingAddress)
        {
            var novaPoshtaSettings = await GetSettings();

            return 2;
        }

        public async Task<int> GetTransitToAddress(Address shippingAddress)
        {
            var novaPoshtaSettings = await GetSettings();

            return 3;
        }

        public async Task<string> GetDeliveryToWarehouseDescription()
        {
            return "На склад:";
        }

        public async Task<ShippingOption> GetToWarehouseShippingOption(Address shippingAddress)
        {
            var option = new ShippingOption
            {
                Name = await _localizationService.GetResourceAsync(
                           "Plugins.Shipping.NovaPoshta.views.shippingMethodName")
                       + ", "
                       + await _localizationService.GetResourceAsync(
                           "Plugins.Shipping.NovaPoshta.views.shippingMethodToWarehouse"),
                Description = $"{shippingAddress.ZipPostalCode}, {shippingAddress.City}:",
                Rate = await GetRateToWarehouse(shippingAddress),
                TransitDays = await GetTransitToWarehouse(shippingAddress),
                ExtendPartialView = "~/Plugins/Shipping.NovaPoshta/Views/ToWarehouseOptionExt.cshtml",
                Address = shippingAddress
            };

            return option;
        }

        public async Task<IList<NovaPoshtaSettlement>> GetSettlementsByAddress(Address address)
        {
            var settlements = await _settlementsRepository
                .GetAllAsync(query =>
                {
                    query = query.Where(settlement =>
                        (settlement.Description == address.City || settlement.DescriptionRu == address.City)
                        && int.Parse(address.ZipPostalCode) >= int.Parse(settlement.Index1)
                        && int.Parse(address.ZipPostalCode) <= int.Parse(settlement.Index2)
                    );

                    return query;
                });

            return settlements;
        }

        public async Task<IList<NovaPoshtaWarehouse>> GetWarehousesBySettlement(NovaPoshtaSettlement settlement)
        {
            var warehousesInSettlement = await _warehousesRepository.GetAllAsync(query =>
            {
                return query.Where(warehouse => warehouse.SettlementRef == settlement.Ref);
            });

            return warehousesInSettlement;
        }

        public async Task<List<NovaPoshtaWarehouse>> GetWarehousesByAddress(Address address)
        {
            var warehouses = new List<NovaPoshtaWarehouse>();
            
            var settlements = await GetSettlementsByAddress(address);
            
            foreach (var settlement in settlements)
            {
                warehouses.AddRange(await GetWarehousesBySettlement(settlement));
            }

            return warehouses;
        }

        public async Task<List<NovaPoshtaConfigurationSettingsModel.WarehouseAvailability>> GetCitiesForSendingAvailability()
        {
            var availability = new List<NovaPoshtaConfigurationSettingsModel.WarehouseAvailability>();

            var nopWarehouses = await _nopWarehousesRepository.GetAllAsync(query => query);

            foreach (var nopWarehouse in nopWarehouses)
            {
                var nopWarehouseAddress = await _nopAddressesRepository.GetByIdAsync(nopWarehouse.AddressId);

                if (nopWarehouseAddress == null) continue;
                
                var novaPoshtaWarehouses = await GetWarehousesByAddress(nopWarehouseAddress);

                availability.Add(new NovaPoshtaConfigurationSettingsModel.WarehouseAvailability
                {
                    Address = nopWarehouseAddress,
                    IsAvailable = novaPoshtaWarehouses.Any(),
                    Warehouse = nopWarehouse,
                    NovaPoshtaWarehousesCount = novaPoshtaWarehouses.Count
                });
            }

            return availability;
        }

        public async Task<ShippingOption> GetToAddressShippingOption(Address shippingAddress)
        {
            var option = new ShippingOption
            {
                Name = await _localizationService.GetResourceAsync(
                           "Plugins.Shipping.NovaPoshta.views.shippingMethodName")
                       + ", "
                       + await _localizationService.GetResourceAsync(
                           "Plugins.Shipping.NovaPoshta.views.shippingMethodAddress"),
                Description =
                    $"{shippingAddress.ZipPostalCode}, {shippingAddress.City}, {shippingAddress.Address1} ({shippingAddress.Address2})",
                Rate = await GetRateToAddress(shippingAddress),
                TransitDays = await GetTransitToAddress(shippingAddress),
                Address = shippingAddress
            };

            return option;
        }

        private async Task<NovaPoshtaSettings> GetSettings()
        {
            return await _settingService.LoadSettingAsync<NovaPoshtaSettings>();
        }
    }
}