using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Shipping;
using Nop.Plugin.Shipping.NovaPoshta.Domain;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Tasks;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public class NovaPoshtaService : INovaPoshtaService
    {
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly INovaPoshtaApiService _novaPoshtaApiService;

        public NovaPoshtaService(
            ISettingService settingService,
            ILocalizationService localizationService,
            INovaPoshtaApiService novaPoshtaApiService,
            IScheduleTaskService scheduleTaskService)
        {
            _settingService = settingService;
            _localizationService = localizationService;
            _novaPoshtaApiService = novaPoshtaApiService;
        }

        public async Task<decimal> GetRateToWarehouse(Address shippingAddress)
        {
            var novaPoshtaSettings = await GetSettings();

            var warehouses = await GetWarehouses(shippingAddress);

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
        
        public async Task<ShippingOption> GetToAddressShippingOption(Address shippingAddress)
        {
            var option = new ShippingOption
            {
                Name = await _localizationService.GetResourceAsync(
                           "Plugins.Shipping.NovaPoshta.views.shippingMethodName")
                       + ", "
                       + await _localizationService.GetResourceAsync(
                           "Plugins.Shipping.NovaPoshta.views.shippingMethodAddress"),
                Description = $"{shippingAddress.ZipPostalCode}, {shippingAddress.City}, {shippingAddress.Address1} ({shippingAddress.Address2})",
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

        private async Task<List<NovaPoshtaWarehouse>> GetWarehouses(Address shippingAddress)
        {
            var warehouses = new List<NovaPoshtaWarehouse>();

            

            return warehouses;
        }
    }
}