namespace Nop.Plugin.Shipping.NovaPoshta.Infrastructure.ApiRequest
{
    public class GetAddressesProps : BaseRequestProps
    {
        public override string ModelName { get; set; } = "Address";
        public override string CalledMethod { get; set; } = "searchSettlements";

        public string CityName { get; set; }
    }
}