namespace Nop.Plugin.Shipping.NovaPoshta.Infrastructure.ApiRequest
{
    public class GetSettlementsProps : BaseRequestProps
    {
        public string Ref { get; set; }
        public int Page { get; set; }
    }
}