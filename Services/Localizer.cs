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
                [LocalizationConst.ShippingMethodName] = "Nova Poshta",
                [LocalizationConst.ShippingMethodToWarehouse] = "to warehouse",
                [LocalizationConst.ShippingMethodAddress] = "to address",
                [LocalizationConst.ApiKey] = "API KEY",
                [LocalizationConst.ApiUrl] = "API URL",
                [LocalizationConst.UseAdditionalFee] = "Use additional fee",
                [LocalizationConst.AdditionalFeeIsPercent] = "Additional commission as a percentage?",
                [LocalizationConst.AdditionalFee] = "Additional fee",
                [LocalizationConst.DbLastSuccessUpdate] = "Last database update",
                [LocalizationConst.WarehouseCities] = "Your warehouses",
                [LocalizationConst.WarehouseUnavailableMessage] = "Some warehouses cannot accept your goods " +
                                                                    "(restrictions on size, weight, or declared value)",
                [LocalizationConst.CreateShipmentWaybill] = "Create new waybill",
                [LocalizationConst.ChangeShippingPoint] = "Change shipping data",
                [LocalizationConst.ShippingDetails] = "Shipping details",
                [LocalizationConst.ShippingDetailsToWarehouse] = "Branch details",
                [LocalizationConst.ShippingDetailsToAddress] = "Shipping address",
                [LocalizationConst.ShippingPointNumber] = "Branch number",
                [LocalizationConst.ShippingPoint] = "Description",
                [LocalizationConst.ZipPostalCode] = "Zip postal code",
                [LocalizationConst.Area] = "Area",
                [LocalizationConst.Region] = "Region",
                [LocalizationConst.City] = "City",
                [LocalizationConst.Street] = "Street",
                [LocalizationConst.House] = "Home",
                [LocalizationConst.Flat] = "Flat",
                [LocalizationConst.PhoneNumber] = "Phone number",
                [LocalizationConst.FirstName] = "First name",
                [LocalizationConst.LastName] = "Last name",
            });

            var allLanguagesAsync = await _languageService.GetAllLanguagesAsync();
            
            foreach (var language in allLanguagesAsync)
            {
                var languageName = _languageService.GetTwoLetterIsoLanguageName(language);
                if (languageName == "ru")
                {
                    await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
                    {
                        [LocalizationConst.ShippingMethodName] = "Новая Почта",
                        [LocalizationConst.ShippingMethodToWarehouse] = "на склад",
                        [LocalizationConst.ShippingMethodAddress] = "на адрес",
                        [LocalizationConst.ApiKey] = "API ключ",
                        [LocalizationConst.ApiUrl] = "API URL",
                        [LocalizationConst.UseAdditionalFee] = "Использовать дополнительную комиссию",
                        [LocalizationConst.AdditionalFeeIsPercent] = "Дополнительная комиссия в процентах?",
                        [LocalizationConst.AdditionalFee] = "Дополнительная комиссия",
                        [LocalizationConst.DbLastSuccessUpdate] = "Последнее обновление базы",
                        [LocalizationConst.WarehouseCities] = "Ваши склады",
                        [LocalizationConst.WarehouseUnavailableMessage] = "Некоторые отделения не могут принять Вашы " +
                                                                            "товары (огрничения по размеру, весу или " +
                                                                            "заявленной стоимости)",
                        [LocalizationConst.CreateShipmentWaybill] = "Создать накладную",
                        [LocalizationConst.ChangeShippingPoint] = "Изменить данные",
                        [LocalizationConst.ShippingDetails] = "Детали доставки",
                        [LocalizationConst.ShippingDetailsToWarehouse] = "Данные отделения доставки",
                        [LocalizationConst.ShippingDetailsToAddress] = "Адрес доставки",
                        [LocalizationConst.ShippingPointNumber] = "Номер отделения",
                        [LocalizationConst.ShippingPoint] = "Описание",
                        [LocalizationConst.ZipPostalCode] = "Индекс",
                        [LocalizationConst.Area] = "Область",
                        [LocalizationConst.Region] = "Район",
                        [LocalizationConst.City] = "Город",
                        [LocalizationConst.Street] = "Улица",
                        [LocalizationConst.House] = "Дом",
                        [LocalizationConst.Flat] = "Квартира",
                        [LocalizationConst.PhoneNumber] = "Номер телефона",
                        [LocalizationConst.FirstName] = "Имя",
                        [LocalizationConst.LastName] = "Фамилия",
                    }, language.Id);
                }
                if (languageName == "uk")
                {
                    await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
                    {
                        [LocalizationConst.ShippingMethodName] = "Нова Пошта",
                        [LocalizationConst.ShippingMethodToWarehouse] = "на склад",
                        [LocalizationConst.ShippingMethodAddress] = "на адресу",
                        [LocalizationConst.ApiKey] = "API ключ",
                        [LocalizationConst.ApiUrl] = "API URL",
                        [LocalizationConst.UseAdditionalFee] = "Використовувати додаткову комісію",
                        [LocalizationConst.AdditionalFeeIsPercent] = "Додаткова комісія у відсотках?",
                        [LocalizationConst.AdditionalFee] = "Додаткова комісія",
                        [LocalizationConst.DbLastSuccessUpdate] = "Останнє оновлення бази",
                        [LocalizationConst.WarehouseCities] = "Ваші склади",
                        [LocalizationConst.WarehouseUnavailableMessage] = "Деякі відділення не можуть прийняти ваші " +
                                                                            "товари (обмеження за розміром, вагою або " +
                                                                            "заявленої вартості)",
                        [LocalizationConst.CreateShipmentWaybill] = "Створити накладну",
                        [LocalizationConst.ChangeShippingPoint] = "Змінити данні",
                        [LocalizationConst.ShippingDetails] = "Подробиці доставки",
                        [LocalizationConst.ShippingPointNumber] = "Номер відділення",
                        [LocalizationConst.ShippingDetailsToWarehouse] = "Дані відділення",
                        [LocalizationConst.ShippingDetailsToAddress] = "Адреса доставки",
                        [LocalizationConst.ShippingPoint] = "Опис",
                        [LocalizationConst.ZipPostalCode] = "Індекс",
                        [LocalizationConst.Area] = "Область",
                        [LocalizationConst.Region] = "Район",
                        [LocalizationConst.City] = "Місто",
                        [LocalizationConst.Street] = "Вулиця",
                        [LocalizationConst.House] = "Будинок",
                        [LocalizationConst.Flat] = "Квартира",
                        [LocalizationConst.PhoneNumber] = "Номер телефону",
                        [LocalizationConst.FirstName] = "Ім'я",
                        [LocalizationConst.LastName] = "Прізвище",
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