using System;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Tasks;
using Nop.Plugin.Shipping.NovaPoshta.Domain;
using Nop.Plugin.Shipping.NovaPoshta.Services;
using Nop.Services.Configuration;
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
        private readonly IScheduleTaskService _scheduleTaskService;
        private readonly ILocalizer _localizer;

        #endregion

        #region Ctor

        public NovaPoshtaComputationMethod(
            IWebHelper webHelper, 
            ISettingService settingService,
            INovaPoshtaService novaPoshtaService,
            IScheduleTaskService scheduleTaskService,
            ILocalizer localizer)
        {
            _webHelper = webHelper;
            _settingService = settingService;
            _novaPoshtaService = novaPoshtaService;
            _scheduleTaskService = scheduleTaskService;
            _localizer = localizer;
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
            
            var toWarehouseShippingOption = await _novaPoshtaService.GetToWarehouseShippingOption(getShippingOptionRequest);
            response.ShippingOptions.Add(toWarehouseShippingOption);

            var toAddressShippingOption = await _novaPoshtaService.GetToAddressShippingOption(getShippingOptionRequest);
            response.ShippingOptions.Add(toAddressShippingOption);

            return response;
        }

        public async Task<decimal?> GetFixedRateAsync(GetShippingOptionRequest getShippingOptionRequest)
        {
            return null;
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/NovaPoshtaShipping/Configure";
        }

        public string GetCheckoutShippingOptionExtPartialViewUrl(string optionType)
        {
            if (optionType == NovaPoshtaShippingType.WAREHOUSE.ToString())
            {
                return "~/Plugins/Shipping.NovaPoshta/Views/_CheckoutToWarehouseOptionExt.cshtml";
            }
            return "";
        }

        public string GetOrderShippingOptionExtPartialViewUrl(string optionType)
        {
            if (optionType == NovaPoshtaShippingType.WAREHOUSE.ToString())
            {
                return "~/Plugins/Shipping.NovaPoshta/Views/_OrderShippingMethodExtPartialView.cshtml";
            }
            
            return "";
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

            await _localizer.SetLocaleResources();

            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            await _localizer.RemoveLocaleResources();

            await RemoveScheduledTasks();
            
            await base.UninstallAsync();
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
        
        private async Task RemoveScheduledTasks()
        {
            var scheduleTask = await _scheduleTaskService.GetTaskByTypeAsync(NovaPoshtaDefaults.UPDATE_DATA_TASK_TYPE);

            if (scheduleTask != null)
            {
                await _scheduleTaskService.DeleteTaskAsync(scheduleTask);
            }
        }
    }
}