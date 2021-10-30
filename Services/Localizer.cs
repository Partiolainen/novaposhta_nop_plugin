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
                [LocalizationConst.SHIPPING_METHOD_NAME] = "Nova Poshta",
                [LocalizationConst.SHIPPING_METHOD_TO_WAREHOUSE] = "to warehouse",
                [LocalizationConst.SHIPPING_METHOD_ADDRESS] = "to address",
                [LocalizationConst.API_KEY] = "API KEY",
                [LocalizationConst.API_URL] = "API URL",
                [LocalizationConst.USE_ADDITIONAL_FEE] = "Use additional fee",
                [LocalizationConst.ADDITIONAL_FEE_IS_PERCENT] = "Additional commission as a percentage?",
                [LocalizationConst.ADDITIONAL_FEE] = "Additional fee",
                [LocalizationConst.DB_LAST_SUCCESS_UPDATE] = "Last database update",
                [LocalizationConst.WAREHOUSE_CITIES] = "Your warehouses",
                [LocalizationConst.WAREHOUSE_UNAVAILABLE_MESSAGE] = "Some warehouses cannot accept your goods " +
                                                                    "(restrictions on size, weight, or declared value)",
                [LocalizationConst.CREATE_SHIPMENT_WAYBILL] = "Create new waybill",
                [LocalizationConst.CHANGE_SHIPPING_POINT] = "Change pickup point",
                [LocalizationConst.SHIPPING_DETAILS] = "Shipping details",
                [LocalizationConst.SHIPPING_POINT_DETAILS] = "Shipping point details",
                [LocalizationConst.SHIPPING_POINT] = "Shipping point",
            });

            var allLanguagesAsync = await _languageService.GetAllLanguagesAsync();
            
            foreach (var language in allLanguagesAsync)
            {
                var languageName = _languageService.GetTwoLetterIsoLanguageName(language);
                if (languageName == "ru")
                {
                    await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
                    {
                        [LocalizationConst.SHIPPING_METHOD_NAME] = "Новая Почта",
                        [LocalizationConst.SHIPPING_METHOD_TO_WAREHOUSE] = "на склад",
                        [LocalizationConst.SHIPPING_METHOD_ADDRESS] = "на адрес",
                        [LocalizationConst.API_KEY] = "API ключ",
                        [LocalizationConst.API_URL] = "API URL",
                        [LocalizationConst.USE_ADDITIONAL_FEE] = "Использовать дополнительную комиссию",
                        [LocalizationConst.ADDITIONAL_FEE_IS_PERCENT] = "Дополнительная комиссия в процентах?",
                        [LocalizationConst.ADDITIONAL_FEE] = "Дополнительная комиссия",
                        [LocalizationConst.DB_LAST_SUCCESS_UPDATE] = "Последнее обновление базы",
                        [LocalizationConst.WAREHOUSE_CITIES] = "Ваши склады",
                        [LocalizationConst.WAREHOUSE_UNAVAILABLE_MESSAGE] = "Некоторые отделения не могут принять Вашы " +
                                                                            "товары (огрничения по размеру, весу или " +
                                                                            "заявленной стоимости)",
                        [LocalizationConst.CREATE_SHIPMENT_WAYBILL] = "Создать накладную",
                        [LocalizationConst.CHANGE_SHIPPING_POINT] = "Изменить точку",
                        [LocalizationConst.SHIPPING_DETAILS] = "Детали доставки",
                        [LocalizationConst.SHIPPING_POINT_DETAILS] = "Данные точки доставки",
                        [LocalizationConst.SHIPPING_POINT] = "Точка доставки",
                    }, language.Id);
                }
                if (languageName == "uk")
                {
                    await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
                    {
                        [LocalizationConst.SHIPPING_METHOD_NAME] = "Нова Пошта",
                        [LocalizationConst.SHIPPING_METHOD_TO_WAREHOUSE] = "на склад",
                        [LocalizationConst.SHIPPING_METHOD_ADDRESS] = "на адресу",
                        [LocalizationConst.API_KEY] = "API ключ",
                        [LocalizationConst.API_URL] = "API URL",
                        [LocalizationConst.USE_ADDITIONAL_FEE] = "Використовувати додаткову комісію",
                        [LocalizationConst.ADDITIONAL_FEE_IS_PERCENT] = "Додаткова комісія у відсотках?",
                        [LocalizationConst.ADDITIONAL_FEE] = "Додаткова комісія",
                        [LocalizationConst.DB_LAST_SUCCESS_UPDATE] = "Останнє оновлення бази",
                        [LocalizationConst.WAREHOUSE_CITIES] = "Ваші склади",
                        [LocalizationConst.WAREHOUSE_UNAVAILABLE_MESSAGE] = "Деякі відділення не можуть прийняти ваші " +
                                                                            "товари (обмеження за розміром, вагою або " +
                                                                            "заявленої вартості)",
                        [LocalizationConst.CREATE_SHIPMENT_WAYBILL] = "Створити накладну",
                        [LocalizationConst.CHANGE_SHIPPING_POINT] = "Змінити точку",
                        [LocalizationConst.SHIPPING_DETAILS] = "Подробиці доставки",
                        [LocalizationConst.SHIPPING_POINT_DETAILS] = "Дані точки доставки",
                        [LocalizationConst.SHIPPING_POINT] = "Точка доставки",
                    }, language.Id);
                }
            }
        }
        
        public async Task RemoveLocaleResources()
        {
            await _localizationService.DeleteLocaleResourcesAsync(LocalizationConst.GetValues());

            var allLanguagesAsync = await _languageService.GetAllLanguagesAsync();
            
            foreach (var language in allLanguagesAsync)
            {
                var languageName = _languageService.GetTwoLetterIsoLanguageName(language);
                
                switch (languageName)
                {
                    case "ru":
                    case "uk":
                        await _localizationService.DeleteLocaleResourcesAsync(LocalizationConst.GetValues(), language.Id);
                        break;
                }
            }
        }
    }
}