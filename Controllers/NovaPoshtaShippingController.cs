using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Shipping;
using Nop.Data;
using Nop.Plugin.Shipping.NovaPoshta.Models;
using Nop.Plugin.Shipping.NovaPoshta.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Tasks;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Task = Nop.Services.Tasks.Task;

namespace Nop.Plugin.Shipping.NovaPoshta.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class NovaPoshtaShippingController : BasePluginController
    {
        private readonly NovaPoshtaSettings _novaPoshtaSettings;
        private readonly ISettingService _settingService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;
        private readonly IScheduleTaskService _scheduleTaskService;
        private readonly INovaPoshtaService _novaPoshtaService;

        private readonly string _endPointBasePath = "~/Plugins/Shipping.NovaPoshta/Views/";

        public NovaPoshtaShippingController(
            NovaPoshtaSettings novaPoshtaSettings, 
            ISettingService settingService,
            INotificationService notificationService,
            ILocalizationService localizationService,
            IScheduleTaskService scheduleTaskService,
            INovaPoshtaService novaPoshtaService)
        {
            _novaPoshtaSettings = novaPoshtaSettings;
            _settingService = settingService;
            _notificationService = notificationService;
            _localizationService = localizationService;
            _scheduleTaskService = scheduleTaskService;
            _novaPoshtaService = novaPoshtaService;
        }

        public async Task<IActionResult> Configure(NovaPoshtaConfigurePageModel pageModel)
        {
            var scheduleTask = await _scheduleTaskService.GetTaskByTypeAsync(NovaPoshtaDefaults.UPDATE_DATA_TASK_TYPE);

            var model = new NovaPoshtaConfigurationSettingsModel
            {
                ApiUrl = _novaPoshtaSettings.ApiUrl,
                ApiKey = _novaPoshtaSettings.ApiKey,
                UseAdditionalFee = _novaPoshtaSettings.UseAdditionalFee,
                AdditionalFee = _novaPoshtaSettings.AdditionalFee,
                AdditionalFeeIsPercent = _novaPoshtaSettings.AdditionalFeeIsPercent,
                DbUpdateStarted = pageModel.DataBaseUpdateStarted,
                DbLastSuccessUpdate = scheduleTask.LastSuccessUtc,
                WarehouseCities = await _novaPoshtaService.GetCitiesForSendingAvailability()
            };

            return View(_endPointBasePath + "Configure.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(NovaPoshtaConfigurationSettingsModel configurationSettingsModel)
        {
            _novaPoshtaSettings.ApiUrl = configurationSettingsModel.ApiUrl;
            _novaPoshtaSettings.ApiKey = configurationSettingsModel.ApiKey;
            _novaPoshtaSettings.UseAdditionalFee = configurationSettingsModel.UseAdditionalFee;
            _novaPoshtaSettings.AdditionalFee = configurationSettingsModel.AdditionalFee;
            _novaPoshtaSettings.AdditionalFeeIsPercent = configurationSettingsModel.AdditionalFeeIsPercent;

            await _settingService.SaveSettingAsync(_novaPoshtaSettings);

            _notificationService.SuccessNotification(
                await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            if (string.IsNullOrEmpty(configurationSettingsModel.ApiUrl) || string.IsNullOrEmpty(configurationSettingsModel.ApiKey)) 
                return await Configure(new NovaPoshtaConfigurePageModel());
            
            RunScheduledTasks();

            return await Configure(new NovaPoshtaConfigurePageModel(true));
        }

        public IActionResult ToWarehouseOptionExt()
        {
            return PartialView(_endPointBasePath + "ToWarehouseOptionExt.cshtml");
        }
        
        [NonAction]
        private async void RunScheduledTasks()
        {
            _notificationService.WarningNotification("Start Nova Poshta warehouses database update process");
            try
            {
                var scheduleTask = await _scheduleTaskService.GetTaskByTypeAsync(NovaPoshtaDefaults.UPDATE_DATA_TASK_TYPE)
                                   ?? throw new ArgumentException("Schedule task cannot be loaded",
                                       NovaPoshtaDefaults.UPDATE_DATA_TASK_TYPE);
                var task = new Task(scheduleTask) {Enabled = true};
                await task.ExecuteAsync(true, false);
            }
            catch (Exception e)
            {
                await _notificationService.ErrorNotificationAsync(e);
            }
        }
    }
}