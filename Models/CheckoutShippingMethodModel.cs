using System.Collections.Generic;
using Nop.Core.Domain.Shipping;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Shipping.NovaPoshta.Models
{
    public partial record CheckoutShippingMethodModel : BaseNopModel
    {
        public CheckoutShippingMethodModel()
        {
            ShippingMethods = new List<ShippingMethodModel>();
            Warnings = new List<string>();
        }

        public IList<ShippingMethodModel> ShippingMethods { get; set; }

        public bool NotifyCustomerAboutShippingFromMultipleLocations { get; set; }

        public IList<string> Warnings { get; set; }

        public bool DisplayPickupInStore { get; set; }

        #region Nested classes

        public partial record ShippingMethodModel : BaseNopModel
        {
            public string ShippingRateComputationMethodSystemName { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Fee { get; set; }
            public bool Selected { get; set; }

            public ShippingOption ShippingOption { get; set; }
        }

        #endregion
    }
}