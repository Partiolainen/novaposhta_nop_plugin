using Nop.Core;

namespace Nop.Plugin.Shipping.NovaPoshta.Domain
{
    public class NpOrderShippingData : BaseEntity
    {
        public int OrderId { get; set; }
        public string ShippingType { get; set; }
        public string NovaPoshtaWarehouseRef { get; set; }
        public int? NovaPoshtaCustomerAddressId { get; set; }
    }
}