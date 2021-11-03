using Nop.Core;

namespace Nop.Plugin.Shipping.NovaPoshta.Domain
{
    public class NpOrderShippingData : BaseEntity
    {
        public int OrderId { get; set; }
        public string ShippingType { get; set; }
        public string NovaPoshtaWarehouseRef { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int? NovaPoshtaCustomerAddressId { get; set; }
    }
}