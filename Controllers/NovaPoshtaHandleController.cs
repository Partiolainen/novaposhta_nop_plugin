using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Shipping;
using Nop.Plugin.Shipping.NovaPoshta.Services;
using Nop.Services.Common;
using Nop.Services.Localization;

namespace Nop.Plugin.Shipping.NovaPoshta.Controllers
{
    public class NovaPoshtaHandleController : Controller
    {
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ILocalizationService _localizationService;

        public NovaPoshtaHandleController(
            IGenericAttributeService genericAttributeService,
            IWorkContext workContext,
            IStoreContext storeContext,
            ILocalizationService localizationService)
        {
            _genericAttributeService = genericAttributeService;
            _workContext = workContext;
            _storeContext = storeContext;
            _localizationService = localizationService;
        }

        [HttpPost]
        public async Task<IActionResult> SelectWarehouse(string warehouseRef)
        {
            var shippingOptions = await _genericAttributeService.GetAttributeAsync<List<ShippingOption>>(
                await _workContext.GetCurrentCustomerAsync(),
                NopCustomerDefaults.OfferedShippingOptionsAttribute,
                (await _storeContext.GetCurrentStoreAsync()).Id);

            var toWarehouse =
                await _localizationService.GetResourceAsync(LocalizationConst.SHIPPING_METHOD_TO_WAREHOUSE);

            var shippingOption = shippingOptions.Find(option => option.Name.Contains(toWarehouse));

            if (shippingOption != null)
                shippingOption.SelectedNpWarehouseRef = warehouseRef;
            else
                throw new Exception("Selected shipping method can't be loaded");

            await _genericAttributeService.SaveAttributeAsync(
                await _workContext.GetCurrentCustomerAsync(),
                NopCustomerDefaults.OfferedShippingOptionsAttribute,
                shippingOptions,
                (await _storeContext.GetCurrentStoreAsync()).Id
            );

            return Ok("success");
        }
    }
}