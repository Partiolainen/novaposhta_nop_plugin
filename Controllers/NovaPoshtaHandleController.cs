using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator.Runner.Generators.Postgres;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Html;
using Nop.Plugin.Shipping.NovaPoshta.Domain;
using Nop.Plugin.Shipping.NovaPoshta.Infrastructure.GenericAttributes;
using Nop.Plugin.Shipping.NovaPoshta.Models;
using Nop.Plugin.Shipping.NovaPoshta.Models.ModelFactories;
using Nop.Plugin.Shipping.NovaPoshta.Services;
using Nop.Plugin.Shipping.NovaPoshta.Services.Factories;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Services.Messages;
using HtmlHelper = Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper;

namespace Nop.Plugin.Shipping.NovaPoshta.Controllers
{
    public class NovaPoshtaHandleController : Controller
    {
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ILocalizationService _localizationService;
        private readonly INpScheduleTasksService _npScheduleTasksService;
        private readonly INotificationServiceExt _notificationServiceExt;
        private readonly IProductService _productService;
        private readonly NovaPoshtaSettings _novaPoshtaSettings;
        private readonly IFactoriesService _factoriesService;
        private readonly INpProductService _npProductService;

        public NovaPoshtaHandleController(
            IGenericAttributeService genericAttributeService,
            IWorkContext workContext,
            IStoreContext storeContext,
            ILocalizationService localizationService,
            INpScheduleTasksService npScheduleTasksService,
            INotificationServiceExt notificationServiceExt,
            IProductService productService,
            NovaPoshtaSettings novaPoshtaSettings,
            IFactoriesService factoriesService,
            INpProductService npProductService)
        {
            _genericAttributeService = genericAttributeService;
            _workContext = workContext;
            _storeContext = storeContext;
            _localizationService = localizationService;
            _npScheduleTasksService = npScheduleTasksService;
            _notificationServiceExt = notificationServiceExt;
            _productService = productService;
            _novaPoshtaSettings = novaPoshtaSettings;
            _factoriesService = factoriesService;
            _npProductService = npProductService;
        }

        [HttpPost]
        public async Task<IActionResult> SelectWarehouse(string warehouseRef, string firstName, string lastName, string phoneNumber)
        {
            var toWarehouseCustomerMainInfo = new ToWarehouseCustomerMainInfo
            {
                WarehouseRef = warehouseRef,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber
            };

            await _genericAttributeService.SaveAttributeAsync(
                await _workContext.GetCurrentCustomerAsync(),
                NovaPoshtaDefaults.CustomerMainInfoForOrder,
                toWarehouseCustomerMainInfo,
                (await _storeContext.GetCurrentStoreAsync()).Id
            );

            return Ok("success");
        }

        [HttpPost]
        public async Task<IActionResult> SaveCheckoutShippingAddress(NpCustomerAddressForOrderModel customerAddressForOrderModel)
        {
            var npCustomerAddressForOrder = new CustomerAddressForOrderModelFactory().BuildAddress(customerAddressForOrderModel);

            await _genericAttributeService.SaveAttributeAsync(
                await _workContext.GetCurrentCustomerAsync(),
                NovaPoshtaDefaults.CustomerAddressForOrder,
                npCustomerAddressForOrder,
                (await _storeContext.GetCurrentStoreAsync()).Id);

            return Ok();
        }
        
        [HttpPost]
        public void UpdateDbNow()
        {
            _notificationServiceExt.NotificationWithSignalR( NotifyType.Success, "Start database update");
            _npScheduleTasksService.UpdateDatabase(true);
        }
        
        [HttpPost]
        public async void CheckProductsDimensionsAndWeight()
        {
            var products = await _productService.SearchProductsAsync();
            var badProducts = new List<Product>();

            foreach (var product in products)
            {
                if (!product.IsShipEnabled) continue;

                await _npProductService.CheckDimensionValuesForZeros(product);

                await _npProductService.CheckWeightAndValueForZero(product);

                if (await _factoriesService.GetNpDimensionsByProduct(product) > _novaPoshtaSettings.GetMaxAllowedDimensions())
                {
                    badProducts.Add(product);
                }

                if (product.Weight > _novaPoshtaSettings.MaxAllowedWeightKg)
                {
                    if (badProducts.FirstOrDefault(prod => prod.Id == product.Id) == null)
                    {
                        badProducts.Add(product);
                    }
                }
                
            }
            
            foreach (var badProduct in badProducts)
            {
                await _notificationServiceExt.NotificationBadProduct(
                    $"Неподходящие параметры размерности или веса ", 
                    badProduct.Id, 
                    badProduct.Name);
            }
        }
    }
}