using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Shipping.NovaPoshta.Models
{
    public class NovaPoshtaModel
    {
        public NovaPoshtaModel()
        {
        }

        [NopResourceDisplayName("Plugins.Shipping.NovaPoshta.Fields.ApiKey")]
        public string ApiKey { get; set; }
        
        [NopResourceDisplayName("Plugins.Shipping.NovaPoshta.Fields.ApiUrl")]
        public string ApiUrl { get; set; }
        
        [NopResourceDisplayName("Plugins.Shipping.NovaPoshta.Fields.UseAdditionalFee")]
        public bool UseAdditionalFee { get; set; }
        
        [NopResourceDisplayName("Plugins.Shipping.NovaPoshta.Fields.AdditionalFeeIsPercent")]
        public bool AdditionalFeeIsPercent { get; set; }
        
        [NopResourceDisplayName("Plugins.Shipping.NovaPoshta.Fields.AdditionalFee")]
        public float AdditionalFee { get; set; }
    }
}