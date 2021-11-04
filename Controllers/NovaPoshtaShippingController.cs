using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Directory;
using Nop.Plugin.Shipping.NovaPoshta.Models;
using Nop.Plugin.Shipping.NovaPoshta.Services;
using Nop.Services.Configuration;
using Nop.Services.Directory;
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
        private readonly IMeasureService _measureService;

        private readonly string _endPointBasePath = "~/Plugins/Shipping.NovaPoshta/Views/";

        public NovaPoshtaShippingController(
            NovaPoshtaSettings novaPoshtaSettings, 
            ISettingService settingService,
            INotificationService notificationService,
            ILocalizationService localizationService,
            IScheduleTaskService scheduleTaskService,
            INpService npService,
            INpScheduleTasksService npScheduleTasksService,
            IMeasureService measureService)
        {
            _novaPoshtaSettings = novaPoshtaSettings;
            _settingService = settingService;
            _notificationService = notificationService;
            _localizationService = localizationService;
            _scheduleTaskService = scheduleTaskService;
            _npService = npService;
            _npScheduleTasksService = npScheduleTasksService;
            _measureService = measureService;
        }

        public async Task<IActionResult> Configure(NovaPoshtaConfigurePageModel pageModel)
        {
            var scheduleTask = await _scheduleTaskService.GetTaskByTypeAsync(NovaPoshtaDefaults.UpdateDataTaskType);

            var (dimensionSelectListItems, centimetresMeasureDimensionId) = await DimensionParamsForModel();

            var (weightSelectListItems, kilogramsMeasureDimensionId) = await WeightParamsForModel();

            var model = new NovaPoshtaConfigurationSettingsModel
            {
                ApiUrl = _novaPoshtaSettings.ApiUrl,
                ApiKey = _novaPoshtaSettings.ApiKey,
                MeasureDimensionId = centimetresMeasureDimensionId,
                AvailableMeasureDimensions = dimensionSelectListItems,
                MeasureWeightId = kilogramsMeasureDimensionId,
                AvailableMeasureWeights = weightSelectListItems,
                UseAdditionalFee = _novaPoshtaSettings.UseAdditionalFee,
                AdditionalFee = _novaPoshtaSettings.AdditionalFee,
                AdditionalFeeIsPercent = _novaPoshtaSettings.AdditionalFeeIsPercent,
                DbUpdateStarted = pageModel.DataBaseUpdateStarted,
                DbLastSuccessUpdate = scheduleTask.LastSuccessUtc,
                WarehouseCities = await _npService.GetCitiesForSendingAvailability()
            };

            return View(_endPointBasePath + "Configure.cshtml", model);
        }

        private async Task<(List<SelectListItem>, int)> DimensionParamsForModel()
        {
            var measureDimensions = await _measureService.GetAllMeasureDimensionsAsync();

            var dimensionSelectListItems = measureDimensions
                .Select(dimension => new SelectListItem(dimension.Name, dimension.Id.ToString()))
                .ToList();

            var centimetresMeasureDimensionId = _novaPoshtaSettings.CentimetresMeasureDimensionId;
            if (centimetresMeasureDimensionId == 0)
            {
                if (!dimensionSelectListItems.Any()) return (dimensionSelectListItems, centimetresMeasureDimensionId);
                
                _novaPoshtaSettings.CentimetresMeasureDimensionId = Convert.ToInt32(dimensionSelectListItems[0].Value);
                await _settingService.SaveSettingAsync(_novaPoshtaSettings);
                dimensionSelectListItems[0].Selected = true;
            }
            else
            {
                if (!dimensionSelectListItems.Any()) return (dimensionSelectListItems, centimetresMeasureDimensionId);
                
                var selectListItem = dimensionSelectListItems
                    .FirstOrDefault(dimSelectItem => dimSelectItem.Value == centimetresMeasureDimensionId.ToString());

                if (selectListItem != null)
                {
                    selectListItem.Selected = true;
                }
            }

            return (dimensionSelectListItems, centimetresMeasureDimensionId);
        }
        
        private async Task<(List<SelectListItem>, int)> WeightParamsForModel()
        {
            var measureWeights = await _measureService.GetAllMeasureWeightsAsync();
            var weightsSelectListItems = measureWeights
                .Select(weight => new SelectListItem(weight.Name, weight.Id.ToString()))
                .ToList();

            var kilogramsMeasureDimensionId = _novaPoshtaSettings.KilogramsMeasureDimensionId;
            if (kilogramsMeasureDimensionId == 0)
            {
                if (!weightsSelectListItems.Any()) return (weightsSelectListItems, kilogramsMeasureDimensionId);
                
                _novaPoshtaSettings.CentimetresMeasureDimensionId = Convert.ToInt32(weightsSelectListItems[0].Value);
                await _settingService.SaveSettingAsync(_novaPoshtaSettings);
                weightsSelectListItems[0].Selected = true;
            }
            else
            {
                if (!weightsSelectListItems.Any()) return (weightsSelectListItems, kilogramsMeasureDimensionId);
                
                var selectListItem = weightsSelectListItems
                    .FirstOrDefault(dimSelectItem => dimSelectItem.Value == kilogramsMeasureDimensionId.ToString());

                if (selectListItem != null)
                {
                    selectListItem.Selected = true;
                }
            }

            return (weightsSelectListItems, kilogramsMeasureDimensionId);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(NovaPoshtaConfigurationSettingsModel configurationSettingsModel)
        {
            _novaPoshtaSettings.ApiUrl = configurationSettingsModel.ApiUrl;
            _novaPoshtaSettings.ApiKey = configurationSettingsModel.ApiKey;
            _novaPoshtaSettings.UseAdditionalFee = configurationSettingsModel.UseAdditionalFee;
            _novaPoshtaSettings.AdditionalFee = configurationSettingsModel.AdditionalFee;
            _novaPoshtaSettings.AdditionalFeeIsPercent = configurationSettingsModel.AdditionalFeeIsPercent;
            _novaPoshtaSettings.CentimetresMeasureDimensionId = configurationSettingsModel.MeasureDimensionId;
            _novaPoshtaSettings.KilogramsMeasureDimensionId = configurationSettingsModel.MeasureWeightId;

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