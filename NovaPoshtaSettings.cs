using Nop.Core.Configuration;
using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta
{
    public class NovaPoshtaSettings : ISettings
    {
        public string ApiKey { get; set; }
        public string ApiUrl { get; set; }
        public int CentimetresMeasureDimensionId { get; set; }
        public int KilogramsMeasureDimensionId { get; set; }
        public int DefaultLengthCm { get; set; }
        public int DefaultWidthCm { get; set; }
        public int DefaultHeightCm { get; set; }
        public int MaxAllowedLengthCm { get; set; }
        public int MaxAllowedWidthCm { get; set; }
        public int MaxAllowedHeightCm { get; set; }
        public int MaxAllowedWeightKg { get; set; }
        public int DefaultWeightKg { get; set; }
        public bool UseAdditionalFee { get; set; }
        public bool AdditionalFeeIsPercent { get; set; }
        public decimal AdditionalFee { get; set; }
        
        public Dimensions GetMaxAllowedDimensions()
        {
            return new Dimensions
            {
                Height = MaxAllowedHeightCm,
                Width = MaxAllowedWidthCm,
                Length = MaxAllowedLengthCm,
            };
        }
        public Dimensions GetDefaultDimensions()
        {
            return new Dimensions
            {
                Height = DefaultHeightCm,
                Width = DefaultWidthCm,
                Length = DefaultLengthCm,
            };
        }
        
    }
}