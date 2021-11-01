using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Shipping.NovaPoshta.Models;
using Nop.Plugin.Shipping.NovaPoshta.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Tasks;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

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
        private readonly INpService _npService;
        private readonly INpScheduleTasksService _npScheduleTasksService;

        private readonly string _endPointBasePath = "~/Plugins/Shipping.NovaPoshta/Views/";

        public NovaPoshtaShippingController(
            NovaPoshtaSettings novaPoshtaSettings, 
            ISettingService settingService,
            INotificationService notificationService,
            ILocalizationService localizationService,
            IScheduleTaskService scheduleTaskService,
            INpService npService,
            INpScheduleTasksService npScheduleTasksService)
        {
            _novaPoshtaSettings = novaPoshtaSettings;
            _settingService = settingService;
            _notificationService = notificationService;
            _localizationService = localizationService;
            _scheduleTaskService = scheduleTaskService;
            _npService = npService;
            _npScheduleTasksService = npScheduleTasksService;
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
                WarehouseCities = await _npService.GetCitiesForSendingAvailability()
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
            
            _npScheduleTasksService.UpdateDatabase();

            return await Configure(new NovaPoshtaConfigurePageModel(true));
        }

        [HttpPost, ActionName("Configure")]
        [FormValueRequired("UpdateDatabase")]
        public async Task<IActionResult> UpdateDatabase(NovaPoshtaConfigurePageModel pageModel)
        {
            _npScheduleTasksService.UpdateDatabase(true);

            return await Configure(pageModel);
        }
    }
}