using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Shipping.NovaPoshta.Infrastructure.GenericAttributes;
using Nop.Plugin.Shipping.NovaPoshta.Models;
using Nop.Plugin.Shipping.NovaPoshta.Models.ModelFactories;
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
    }
}