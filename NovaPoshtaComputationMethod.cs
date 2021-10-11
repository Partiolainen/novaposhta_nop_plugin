using System;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Services.Plugins;
using Nop.Services.Shipping;
using Nop.Services.Shipping.Tracking;

namespace Nop.Plugin.Shipping.NovaPoshta
{
    public class NovaPoshtaComputationMethod : BasePlugin, IShippingRateComputationMethod
    {
        private readonly IWebHelper _webHelper;

        public NovaPoshtaComputationMethod(IWebHelper webHelper)
        {
            _webHelper = webHelper;
        }

        public IShipmentTracker ShipmentTracker { get; }

        public Task<GetShippingOptionResponse> GetShippingOptionsAsync(GetShippingOptionRequest getShippingOptionRequest)
        {
            throw new NotImplementedException();
        }

        public Task<decimal?> GetFixedRateAsync(GetShippingOptionRequest getShippingOptionRequest)
        {
            throw new NotImplementedException();
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/NovaPoshtaShipping/Configure";
        }
    }
}