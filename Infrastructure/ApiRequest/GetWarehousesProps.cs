namespace Nop.Plugin.Shipping.NovaPoshta.Infrastructure.ApiRequest
{
    public class GetWarehousesProps : BaseRequestProps
    {
        public override string ModelName { get; set; } = "AddressGeneral";
        public override string CalledMethod { get; set; } = "getWarehouses";
        public string CityRef { get; set; }
    }
}