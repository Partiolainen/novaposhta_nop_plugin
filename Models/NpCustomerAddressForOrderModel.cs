namespace Nop.Plugin.Shipping.NovaPoshta.Models
{
    public class NpCustomerAddressForOrderModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string ZipPostalCode { get; set; }
        public string Area { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Flat { get; set; }
    }
}