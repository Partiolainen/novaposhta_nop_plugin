using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Shipping.NovaPoshta.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
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

        private readonly string _endPointBasePath = "~/Plugins/Shipping.NovaPoshta/Views/";

        public NovaPoshtaShippingController(
            NovaPoshtaSettings novaPoshtaSettings, 
            ISettingService settingService,
            INotificationService notificationService,
            ILocalizationService localizationService)
        {
            _novaPoshtaSettings = novaPoshtaSettings;
            _settingService = settingService;
            _notificationService = notificationService;
            _localizationService = localizationService;
        }

        public async Task<IActionResult> Configure()
        {
            var model = new NovaPoshtaModel
            {
                ApiUrl = _novaPoshtaSettings.ApiUrl,
                ApiKey = _novaPoshtaSettings.ApiKey,
                UseAdditionalFee = _novaPoshtaSettings.UseAdditionalFee,
                AdditionalFee = _novaPoshtaSettings.AdditionalFee,
                AdditionalFeeIsPercent = _novaPoshtaSettings.AdditionalFeeIsPercent
            };

            return View(_endPointBasePath + "Configure.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(NovaPoshtaModel model)
        {
            _novaPoshtaSettings.ApiUrl = model.ApiUrl;
            _novaPoshtaSettings.ApiKey = model.ApiKey;
            _novaPoshtaSettings.UseAdditionalFee = model.UseAdditionalFee;
            _novaPoshtaSettings.AdditionalFee = model.AdditionalFee;
            _novaPoshtaSettings.AdditionalFeeIsPercent = model.AdditionalFeeIsPercent;

            await _settingService.SaveSettingAsync(_novaPoshtaSettings);

            _notificationService.SuccessNotification(
                await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));
            
            return await Configure();
        }

        public IActionResult ToWarehouseOptionExt()
        {
            return PartialView(_endPointBasePath + "ToWarehouseOptionExt.cshtml");
        }
    }
}