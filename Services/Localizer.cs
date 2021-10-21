using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Services.Localization;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public class Localizer : ILocalizer
    {
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;

        public Localizer(ILocalizationService localizationService, ILanguageService languageService)
        {
            _localizationService = localizationService;
            _languageService = languageService;
        }

        public async Task SetLocaleResources()
        {
            await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
            {
                [NovaPoshtaDefaults.SHIPPING_METHOD_NAME] = "Nova Poshta",
                [NovaPoshtaDefaults.SHIPPING_METHOD_TO_WAREHOUSE] = "to warehouse",
                [NovaPoshtaDefaults.SHIPPING_METHOD_ADDRESS] = "to address",
                [NovaPoshtaDefaults.API_KEY] = "API KEY",
                [NovaPoshtaDefaults.API_URL] = "API URL",
                [NovaPoshtaDefaults.USE_ADDITIONAL_FEE] = "Use additional fee",
                [NovaPoshtaDefaults.ADDITIONAL_FEE_IS_PERCENT] = "Additional commission as a percentage?",
                [NovaPoshtaDefaults.ADDITIONAL_FEE] = "Additional fee",
            });

            var allLanguagesAsync = await _languageService.GetAllLanguagesAsync();
            
            foreach (var language in allLanguagesAsync)
            {
                var languageName = _languageService.GetTwoLetterIsoLanguageName(language);
                if (languageName == "ru")
                {
                    await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
                    {
                        [NovaPoshtaDefaults.SHIPPING_METHOD_NAME] = "Новая Почта",
                        [NovaPoshtaDefaults.SHIPPING_METHOD_TO_WAREHOUSE] = "на склад",
                        [NovaPoshtaDefaults.SHIPPING_METHOD_ADDRESS] = "на адрес",
                        [NovaPoshtaDefaults.API_KEY] = "API ключ",
                        [NovaPoshtaDefaults.API_URL] = "API URL",
                        [NovaPoshtaDefaults.USE_ADDITIONAL_FEE] = "Использовать дополнительную комиссию",
                        [NovaPoshtaDefaults.ADDITIONAL_FEE_IS_PERCENT] = "Дополнительная комиссия в процентах?",
                        [NovaPoshtaDefaults.ADDITIONAL_FEE] = "Дополнительная комиссия"
                    }, language.Id);
                }
                if (languageName == "uk")
                {
                    await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
                    {
                        [NovaPoshtaDefaults.SHIPPING_METHOD_NAME] = "Нова Пошта",
                        [NovaPoshtaDefaults.SHIPPING_METHOD_TO_WAREHOUSE] = "на склад",
                        [NovaPoshtaDefaults.SHIPPING_METHOD_ADDRESS] = "на адресу",
                        [NovaPoshtaDefaults.API_KEY] = "API ключ",
                        [NovaPoshtaDefaults.API_URL] = "API URL",
                        [NovaPoshtaDefaults.USE_ADDITIONAL_FEE] = "Використовувати додаткову комісію",
                        [NovaPoshtaDefaults.ADDITIONAL_FEE_IS_PERCENT] = "Додаткова комісія у відсотках?",
                        [NovaPoshtaDefaults.ADDITIONAL_FEE] = "Додаткова комісія",
                    }, language.Id);
                }
            }
        }
        
        public async Task RemoveLocaleResources()
        {
            await _localizationService.DeleteLocaleResourcesAsync(new List<string>
            {
                NovaPoshtaDefaults.SHIPPING_METHOD_NAME,
                NovaPoshtaDefaults.SHIPPING_METHOD_TO_WAREHOUSE,
                NovaPoshtaDefaults.SHIPPING_METHOD_ADDRESS,
                NovaPoshtaDefaults.API_KEY,
                NovaPoshtaDefaults.API_URL,
                NovaPoshtaDefaults.USE_ADDITIONAL_FEE,
                NovaPoshtaDefaults.ADDITIONAL_FEE_IS_PERCENT,
                NovaPoshtaDefaults.ADDITIONAL_FEE,
            });

            var allLanguagesAsync = await _languageService.GetAllLanguagesAsync();
            
            foreach (var language in allLanguagesAsync)
            {
                var languageName = _languageService.GetTwoLetterIsoLanguageName(language);
                if (languageName == "ru")
                {
                    await _localizationService.DeleteLocaleResourcesAsync(new List<string>
                    {
                        NovaPoshtaDefaults.SHIPPING_METHOD_NAME,
                        NovaPoshtaDefaults.SHIPPING_METHOD_TO_WAREHOUSE,
                        NovaPoshtaDefaults.SHIPPING_METHOD_ADDRESS,
                        NovaPoshtaDefaults.API_KEY,
                        NovaPoshtaDefaults.API_URL,
                        NovaPoshtaDefaults.USE_ADDITIONAL_FEE,
                        NovaPoshtaDefaults.ADDITIONAL_FEE_IS_PERCENT,
                        NovaPoshtaDefaults.ADDITIONAL_FEE,
                    }, language.Id);
                }
                if (languageName == "uk")
                {
                    await _localizationService.DeleteLocaleResourcesAsync(new List<string>
                    {
                        NovaPoshtaDefaults.SHIPPING_METHOD_NAME,
                        NovaPoshtaDefaults.SHIPPING_METHOD_TO_WAREHOUSE,
                        NovaPoshtaDefaults.SHIPPING_METHOD_ADDRESS,
                        NovaPoshtaDefaults.API_KEY,
                        NovaPoshtaDefaults.API_URL,
                        NovaPoshtaDefaults.USE_ADDITIONAL_FEE,
                        NovaPoshtaDefaults.ADDITIONAL_FEE_IS_PERCENT,
                        NovaPoshtaDefaults.ADDITIONAL_FEE,
                    }, language.Id);
                }
            }
        }
    }
}