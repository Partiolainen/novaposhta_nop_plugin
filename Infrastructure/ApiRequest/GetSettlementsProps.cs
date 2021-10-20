namespace Nop.Plugin.Shipping.NovaPoshta.Infrastructure.ApiRequest
{
    public class GetSettlementsProps : BaseRequestProps
    {
        public override string ModelName { get; set; } = "AddressGeneral";
        public override string CalledMethod { get; set; } = "getSettlements";

        public string @Ref { get; set; }
    }
}