using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.ExportImport;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Security;
using Nop.Services.Shipping;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Areas.Admin.Models.Orders;
using Nop.Web.Framework.Controllers;
using IOrderModelFactory = Nop.Web.Areas.Admin.Factories.IOrderModelFactory;

namespace Nop.Plugin.Shipping.NovaPoshta.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/Order/[action]/{id?}")]
    public class NpPluginOrdersController : OrderController
    {
        private readonly IOrderModelFactory _orderModelFactory;
        private readonly IOrderService _orderService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly IServiceCollection _serviceCollection;

        public NpPluginOrdersController(
            IAddressAttributeParser addressAttributeParser,
            IAddressService addressService,
            ICustomerActivityService customerActivityService,
            ICustomerService customerService,
            IDateTimeHelper dateTimeHelper,
            IDownloadService downloadService,
            IEncryptionService encryptionService,
            IExportManager exportManager,
            IGiftCardService giftCardService,
            ILocalizationService localizationService,
            INotificationService notificationService,
            IOrderModelFactory orderModelFactory,
            IOrderProcessingService orderProcessingService,
            IOrderService orderService,
            IPaymentService paymentService,
            IPdfService pdfService,
            IPermissionService permissionService,
            IPriceCalculationService priceCalculationService, IProductAttributeFormatter productAttributeFormatter,
            IProductAttributeParser productAttributeParser, IProductAttributeService productAttributeService,
            IProductService productService, IShipmentService shipmentService, IShippingService shippingService,
            IShoppingCartService shoppingCartService, IWorkContext workContext,
            IWorkflowMessageService workflowMessageService, OrderSettings orderSettings, IServiceCollection serviceCollection) 
            : base(addressAttributeParser,
            addressService, customerActivityService, customerService, dateTimeHelper, downloadService,
            encryptionService, exportManager, giftCardService, localizationService, notificationService,
            orderModelFactory, orderProcessingService, orderService, paymentService, pdfService, permissionService,
            priceCalculationService, productAttributeFormatter, productAttributeParser, productAttributeService,
            productService, shipmentService, shippingService, shoppingCartService, workContext, workflowMessageService,
            orderSettings)
        {
            _orderModelFactory = orderModelFactory;
            _orderService = orderService;
            _permissionService = permissionService;
            _workContext = workContext;
            _serviceCollection = serviceCollection;
        }
        
        public override async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();
        
            // var actionResult = await base.Edit(id);
        
            //try to get an order with the specified id
            
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null || order.Deleted)
                return RedirectToAction("List");
            
            //a vendor does not have access to this functionality
            if (await _workContext.GetCurrentVendorAsync() != null && !await HasAccessToOrderAsync(order))
                return RedirectToAction("List");
            
            //prepare model
            var model = await _orderModelFactory.PrepareOrderModelAsync(null, order);
        
            // return View("~/Plugins/Shipping.NovaPoshta/Areas/Admin/Views/NpPluginOrders/Edit.cshtml", model);
            return View(model);
        }

        

        

        private IActionResult BuildViewResult(IActionResult actionResult, string viewFile)
        {
            ((ViewResult)actionResult).ViewName =
                $"~/Plugins/Shipping.NovaPoshta/Areas/Admin/Views/NpPluginOrders/{viewFile}.cshtml";

            return actionResult;
        }
    }
}