using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.Shipping;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Shipping;
using Nop.Web.Controllers;
using Nop.Web.Factories;

namespace Nop.Plugin.Shipping.NovaPoshta.Controllers.Inheritors
{
    [Route("/checkout/[action]/{orderId?}")]
    public class NpCheckoutController : CheckoutController
    {
        public NpCheckoutController(AddressSettings addressSettings, CustomerSettings customerSettings,
            IAddressAttributeParser addressAttributeParser, IAddressService addressService,
            ICheckoutModelFactory checkoutModelFactory, ICountryService countryService,
            ICustomerService customerService, IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService, ILogger logger, IOrderProcessingService orderProcessingService,
            IOrderService orderService, IPaymentPluginManager paymentPluginManager, IPaymentService paymentService,
            IProductService productService, IShippingService shippingService, IShoppingCartService shoppingCartService,
            IStoreContext storeContext, IWebHelper webHelper, IWorkContext workContext, OrderSettings orderSettings,
            PaymentSettings paymentSettings, RewardPointsSettings rewardPointsSettings,
            ShippingSettings shippingSettings) : base(addressSettings, customerSettings, addressAttributeParser,
            addressService, checkoutModelFactory, countryService, customerService, genericAttributeService,
            localizationService, logger, orderProcessingService, orderService, paymentPluginManager, paymentService,
            productService, shippingService, shoppingCartService, storeContext, webHelper, workContext, orderSettings,
            paymentSettings, rewardPointsSettings, shippingSettings)
        {
        }

        [Route("/onepagecheckout")]
        public override Task<IActionResult> OnePageCheckout()
        {
            return base.OnePageCheckout();
        }
    }
}