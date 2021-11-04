using Nop.Core.Configuration;

namespace Nop.Plugin.Shipping.NovaPoshta
{
    public class NovaPoshtaSettings : ISettings
    {
        public string ApiKey { get; set; }
        public string ApiUrl { get; set; }
        public int CentimetresMeasureDimensionId { get; set; }
        public int KilogramsMeasureDimensionId { get; set; }
        public bool UseAdditionalFee { get; set; }
        public bool AdditionalFeeIsPercent { get; set; }
        public decimal AdditionalFee { get; set; }
    }
}