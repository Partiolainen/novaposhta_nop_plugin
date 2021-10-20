namespace Nop.Plugin.Shipping.NovaPoshta.Infrastructure.ApiRequest
{
    public abstract class BaseRequestProps
    {
        public abstract string ModelName { get; set; }
        public abstract string CalledMethod { get; set; }
    }
}