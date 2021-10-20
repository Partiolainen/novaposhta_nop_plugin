using Nop.Core.Configuration;

namespace Nop.Plugin.Shipping.NovaPoshta
{
    public class NovaPoshtaSettings : ISettings
    {
        public string ApiKey { get; set; }
        public string ApiUrl { get; set; }
        public bool UseAdditionalFee { get; set; }
        public bool AdditionalFeeIsPercent { get; set; }
        public float AdditionalFee { get; set; }
    }
}