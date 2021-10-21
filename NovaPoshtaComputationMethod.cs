using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Tasks;
using Nop.Plugin.Shipping.NovaPoshta.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Shipping;
using Nop.Services.Shipping.Tracking;
using Nop.Services.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Nop.Plugin.Shipping.NovaPoshta
{
    public class NovaPoshtaComputationMethod : BasePlugin, IShippingRateComputationMethod
    {
        #region Fields

        private readonly IWebHelper _webHelper;
        private readonly ISettingService _settingService;
        private readonly INovaPoshtaService _novaPoshtaService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IScheduleTaskService _scheduleTaskService;

        #endregion

        #region Ctor

        public NovaPoshtaComputationMethod(
            IWebHelper webHelper, 
            ISettingService settingService,
            INovaPoshtaService novaPoshtaService,
            ILocalizationService localizationService,
            ILanguageService languageService,
            IScheduleTaskService scheduleTaskService)
        {
            _webHelper = webHelper;
            _settingService = settingService;
            _novaPoshtaService = novaPoshtaService;
            _localizationService = localizationService;
            _languageService = languageService;
            _scheduleTaskService = scheduleTaskService;
        }
        
        #endregion
        
        public IShipmentTracker ShipmentTracker { get; }

        public async Task<GetShippingOptionResponse> GetShippingOptionsAsync(GetShippingOptionRequest getShippingOptionRequest)
        {
            if (getShippingOptionRequest == null)
            {
                throw new ArgumentNullException(nameof(getShippingOptionRequest));
            }

            var response = new GetShippingOptionResponse();
            
            if (getShippingOptionRequest.Items == null || !getShippingOptionRequest.Items.Any())
            {
                response.AddError("No shipment items");
                return response;
            }
            
            var toWarehouseShippingOption = await _novaPoshtaService.GetToWarehouseShippingOption(getShippingOptionRequest.ShippingAddress);
            response.ShippingOptions.Add(toWarehouseShippingOption);

            var toAddressShippingOption = await _novaPoshtaService.GetToAddressShippingOption(getShippingOptionRequest.ShippingAddress);
            response.ShippingOptions.Add(toAddressShippingOption);

            return response;
        }

        public async Task<decimal?> GetFixedRateAsync(GetShippingOptionRequest getShippingOptionRequest)
        {
            return 90;
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/NovaPoshtaShipping/Configure";
        }

        public override async Task InstallAsync()
        {
            await _settingService.SaveSettingAsync(new NovaPoshtaSettings
            {
                ApiKey = "",
                ApiUrl = "https://api.novaposhta.ua/v2.0/json/",
                AdditionalFee = 0,
                UseAdditionalFee = false,
                AdditionalFeeIsPercent = false
            });

            await InstallScheduledTasks();

            await SetLocaleResources();

            await base.InstallAsync();
        }
        
        private async Task InstallScheduledTasks()
        {
            if (await _scheduleTaskService.GetTaskByTypeAsync(NovaPoshtaDefaults.UPDATE_DATA_TASK_TYPE) == null)
            {
                await _scheduleTaskService.InsertTaskAsync(new ScheduleTask
                {
                    Enabled = true,
                    Seconds = NovaPoshtaDefaults.DEFAULT_SYNCHRONIZATION_PERIOD * 60 * 60,
                    Name = NovaPoshtaDefaults.SYNCHRONIZATION_TASK_NAME,
                    Type = NovaPoshtaDefaults.UPDATE_DATA_TASK_TYPE
                });
            }
        }
        
        private async Task SetLocaleResources()
        {
            await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Shipping.NovaPoshta.views.shippingMethodName"] = "Nova Poshta",
                ["Plugins.Shipping.NovaPoshta.views.shippingMethodToWarehouse"] = "to warehouse",
                ["Plugins.Shipping.NovaPoshta.views.shippingMethodAddress"] = "to address",
                ["Plugins.Shipping.NovaPoshta.Fields.ApiKey"] = "API KEY",
                ["Plugins.Shipping.NovaPoshta.Fields.ApiUrl"] = "API URL",
                ["Plugins.Shipping.NovaPoshta.Fields.UseAdditionalFee"] = "Use additional fee",
                ["Plugins.Shipping.NovaPoshta.Fields.AdditionalFeeIsPercent"] = "Additional commission as a percentage?",
                ["Plugins.Shipping.NovaPoshta.Fields.AdditionalFee"] = "Additional fee",
            });
            
            var allLanguagesAsync = await _languageService.GetAllLanguagesAsync();
            
            foreach (var language in allLanguagesAsync)
            {
                var languageName = _languageService.GetTwoLetterIsoLanguageName(language);
                if (languageName == "ru")
                {
                    await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
                    {
                        ["Plugins.Shipping.NovaPoshta.views.shippingMethodName"] = "Новая Почта",
                        ["Plugins.Shipping.NovaPoshta.views.shippingMethodToWarehouse"] = "на склад",
                        ["Plugins.Shipping.NovaPoshta.views.shippingMethodAddress"] = "на адрес",
                        ["Plugins.Shipping.NovaPoshta.Fields.ApiKey"] = "API ключ",
                        ["Plugins.Shipping.NovaPoshta.Fields.ApiUrl"] = "API URL",
                        ["Plugins.Shipping.NovaPoshta.Fields.UseAdditionalFee"] = "Использовать дополнительную комиссию",
                        ["Plugins.Shipping.NovaPoshta.Fields.AdditionalFeeIsPercent"] = "Дополнительная комиссия в процентах?",
                        ["Plugins.Shipping.NovaPoshta.Fields.AdditionalFee"] = "Дополнительная комиссия"
                    }, language.Id);
                }
                if (languageName == "uk")
                {
                    await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
                    {
                        ["Plugins.Shipping.NovaPoshta.views.shippingMethodName"] = "Нова Пошта",
                        ["Plugins.Shipping.NovaPoshta.views.shippingMethodToWarehouse"] = "на склад",
                        ["Plugins.Shipping.NovaPoshta.views.shippingMethodAddress"] = "на адресу",
                        ["Plugins.Shipping.NovaPoshta.Fields.ApiKey"] = "API ключ",
                        ["Plugins.Shipping.NovaPoshta.Fields.ApiUrl"] = "API URL",
                        ["Plugins.Shipping.NovaPoshta.Fields.UseAdditionalFee"] = "Використовувати додаткову комісію",
                        ["Plugins.Shipping.NovaPoshta.Fields.AdditionalFeeIsPercent"] = "Додаткова комісія у відсотках?",
                        ["Plugins.Shipping.NovaPoshta.Fields.AdditionalFee"] = "Додаткова комісія",
                    }, language.Id);
                }
            }
        }
    }
}