using System.Collections.Generic;

namespace Nop.Plugin.Shipping.NovaPoshta.Infrastructure.ApiRequest
{
    public class GetDeliveryCostProps : BaseRequestProps
    {
        public string CitySender { get; set; }
        public string CityRecipient { get; set; }
        public string Weight { get; set; }
        public string ServiceType { get; set; }
        public string Cost { get; set; }
        public string CargoType { get; set; }
        public string SeatsAmount { get; set; }
        public List<OptionSeat> OptionsSeat { get; set; }
    }

    public record OptionSeat
    {
        public string weight { get; set; }
        public string volumetricWidth { get; set; }
        public string volumetricLength { get; set; }
        public string volumetricHeight { get; set; }
    }
}