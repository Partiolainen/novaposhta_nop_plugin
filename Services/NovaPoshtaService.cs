using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator.Runner.Generators.Postgres;
using LinqToDB.Reflection;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Shipping;
using Nop.Data;
using Nop.Plugin.Shipping.NovaPoshta.Data;
using Nop.Plugin.Shipping.NovaPoshta.Domain;
using Nop.Plugin.Shipping.NovaPoshta.Models;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Shipping;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public class NovaPoshtaService : INovaPoshtaService
    {
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly INovaPoshtaRepository<NovaPoshtaSettlement> _settlementsRepository;
        private readonly INovaPoshtaRepository<NovaPoshtaWarehouse> _warehousesRepository;
        private readonly IRepository<Warehouse> _nopWarehousesRepository;
        private readonly IAddressService _addressService;
        private readonly INovaPoshtaApiService _novaPoshtaApiService;

        public NovaPoshtaService(
            ISettingService settingService,
            ILocalizationService localizationService,
            INovaPoshtaRepository<NovaPoshtaSettlement> settlementsRepository,
            INovaPoshtaRepository<NovaPoshtaWarehouse> warehousesRepository,
            IRepository<Warehouse> nopWarehousesRepository,
            IAddressService addressService,
            INovaPoshtaApiService novaPoshtaApiService)
        {
            _settingService = settingService;
            _localizationService = localizationService;
            _settlementsRepository = settlementsRepository;
            _warehousesRepository = warehousesRepository;
            _nopWarehousesRepository = nopWarehousesRepository;
            _addressService = addressService;
            _novaPoshtaApiService = novaPoshtaApiService;
        }

        public async Task<ShippingOption> GetToWarehouseShippingOption(
            GetShippingOptionRequest getShippingOptionRequest)
        {
            var shippingAddress = getShippingOptionRequest.ShippingAddress;

            var option = new ShippingOption
            {
                Name = await _localizationService.GetResourceAsync(
                           "Plugins.Shipping.NovaPoshta.views.shippingMethodName")
                       + ", "
                       + await _localizationService.GetResourceAsync(
                           "Plugins.Shipping.NovaPoshta.views.shippingMethodToWarehouse"),
                ShippingType = NovaPoshtaShippingType.WAREHOUSE.ToString(),
                Description = $"{shippingAddress.ZipPostalCode}, {shippingAddress.City}",
                Rate = await GetRateToWarehouse(getShippingOptionRequest),
                TransitDays = 2,
                CheckoutShippingOptionsExtPartialView = "~/Plugins/Shipping.NovaPoshta/Views/_CheckoutToWarehouseOptionExt.cshtml",
                OrderShippingMethodExtPartialView = "~/Plugins/Shipping.NovaPoshta/Views/_OrderShippingMethodExtPartialView.cshtml",
                Address = shippingAddress,
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

        public async Task<List<NovaPoshtaConfigurationSettingsModel.WarehouseAvailability>>
            GetCitiesForSendingAvailability()
        {
            var availability = new List<NovaPoshtaConfigurationSettingsModel.WarehouseAvailability>();

            var nopWarehouses = await _nopWarehousesRepository.GetAllAsync(query => query);

            foreach (var nopWarehouse in nopWarehouses)
            {
                var nopWarehouseAddress = await _addressService.GetAddressByIdAsync(nopWarehouse.AddressId);

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

        public async Task<ShippingOption> GetToAddressShippingOption(GetShippingOptionRequest getShippingOptionRequest)
        {
            var shippingAddress = getShippingOptionRequest.ShippingAddress;

            var option = new ShippingOption
            {
                Name = await _localizationService.GetResourceAsync(
                           "Plugins.Shipping.NovaPoshta.views.shippingMethodName")
                       + ", "
                       + await _localizationService.GetResourceAsync(
                           "Plugins.Shipping.NovaPoshta.views.shippingMethodAddress"),
                ShippingType = NovaPoshtaShippingType.ADDRESS.ToString(),
                Description =
                    $"{shippingAddress.ZipPostalCode}, {shippingAddress.City}, {shippingAddress.Address1} ({shippingAddress.Address2})",
                Rate = await GetRateToAddress(getShippingOptionRequest),
                TransitDays = 3,
                Address = shippingAddress,
            };

            return option;
        }

        private async Task<NovaPoshtaSettings> GetSettings()
        {
            return await _settingService.LoadSettingAsync<NovaPoshtaSettings>();
        }

        private async Task<decimal> GetRateToWarehouse(GetShippingOptionRequest request)
        {
            decimal resultRate = 0;
            
            var settings = await GetSettings();

            if (settings.UseAdditionalFee)
            {
                resultRate += settings.AdditionalFeeIsPercent
                    ? Math.Round(resultRate * settings.AdditionalFee * (decimal)0.01)
                    : settings.AdditionalFee;
            }

            var warehouseAddress = await ExtractWarehouseAddress(request);

            var sender = (await GetSettlementsByAddress(warehouseAddress)).First();
            var recipient = (await GetSettlementsByAddress(request.ShippingAddress)).First();

            foreach (var requestItem in request.Items)
            {
                var price = 
                    await _novaPoshtaApiService.GetDeliveryPrice(sender, recipient, requestItem.Product);
                if (price.Any())
                {
                    resultRate += price.First().Cost * requestItem.GetQuantity();
                }
                else
                {
                    resultRate = 0;
                }
            }

            return resultRate;
        }

        private async Task<decimal> GetRateToAddress(GetShippingOptionRequest request)
        {
            decimal resultRate = 0;
            
            var settings = await GetSettings();

            if (settings.UseAdditionalFee)
            {
                resultRate += settings.AdditionalFeeIsPercent
                    ? Math.Round(resultRate * settings.AdditionalFee * (decimal)0.01)
                    : settings.AdditionalFee;
            }

            var warehouseAddress =  await ExtractWarehouseAddress(request);
            var sender = (await GetSettlementsByAddress(warehouseAddress)).First();
            var recipient = (await GetSettlementsByAddress(request.ShippingAddress)).First();

            foreach (var requestItem in request.Items)
            {
                var price = 
                    await _novaPoshtaApiService.GetDeliveryPrice(sender, recipient, requestItem.Product, false);
                if (price.Any())
                {
                    resultRate += price.First().Cost * requestItem.GetQuantity();
                }
                else
                {
                    resultRate = 0;
                }
            }

            return resultRate;
        }
        
        private async Task<Address> ExtractWarehouseAddress(GetShippingOptionRequest request)
        {
            Address warehouseAddress;
            if (request.WarehouseFrom != null)
            {
                warehouseAddress = await _addressService.GetAddressByIdAsync(request.WarehouseFrom.AddressId);
            }
            else
            {
                var warehouseId = (await _nopWarehousesRepository.GetAllAsync(queryable => queryable)).First().AddressId;
                if (warehouseId == 0)
                    throw new ArgumentException("Warehouse not found");

                warehouseAddress = await _addressService.GetAddressByIdAsync(warehouseId);
            }

            return warehouseAddress;
        }
    }
}