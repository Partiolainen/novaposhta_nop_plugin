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
        private readonly INpService _npService;
        private readonly IScheduleTaskService _scheduleTaskService;
        private readonly ILocalizer _localizer;

        #endregion

        #region Ctor

        public NovaPoshtaComputationMethod(
            IWebHelper webHelper, 
            ISettingService settingService,
            INpService npService,
            IScheduleTaskService scheduleTaskService,
            ILocalizer localizer)
        {
            _webHelper = webHelper;
            _settingService = settingService;
            _npService = npService;
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
            
            var toWarehouseShippingOption = await _npService.GetToWarehouseShippingOption(getShippingOptionRequest);
            response.ShippingOptions.Add(toWarehouseShippingOption);

            var toAddressShippingOption = await _npService.GetToAddressShippingOption(getShippingOptionRequest);
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

        public string GetCheckoutShippingOptionExtPartialView()
        {
            return "~/Plugins/Shipping.NovaPoshta/Views/_CheckoutShippingMainAreaPartialView.cshtml";
        }
        
        public string GetOrderSummaryShippingOptionExtPartialView()
        {
            return "~/Plugins/Shipping.NovaPoshta/Views/_CheckoutOrderSummaryShippingPartialView.cshtml";
        }

        public string GetAdminAreaOrderShippingOptionExtPartialView()
        {
            return "~/Plugins/Shipping.NovaPoshta/Views/_OrderShippingAdminAreaPartialView.cshtml";
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
            if (await _scheduleTaskService.GetTaskByTypeAsync(NovaPoshtaDefaults.UpdateDataTaskType) == null)
            {
                await _scheduleTaskService.InsertTaskAsync(new ScheduleTask
                {
                    Enabled = true,
                    Seconds = NovaPoshtaDefaults.DefaultSynchronizationPeriod * 60 * 60,
                    Name = NovaPoshtaDefaults.SynchronizationTaskName,
                    Type = NovaPoshtaDefaults.UpdateDataTaskType
                });
            }
        }
        
        private async Task RemoveScheduledTasks()
        {
            var scheduleTask = await _scheduleTaskService.GetTaskByTypeAsync(NovaPoshtaDefaults.UpdateDataTaskType);

            if (scheduleTask != null)
            {
                await _scheduleTaskService.DeleteTaskAsync(scheduleTask);
            }
        }
    }
}