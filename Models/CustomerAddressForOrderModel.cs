using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta.Models
{
    public class CustomerAddressForOrderModel
    {
        public CustomerAddressForOrderModel(NpCustomerAddressForOrder customerAddressForOrder)
        {
            CustomerAddressForOrder = customerAddressForOrder;
        }

        public NpCustomerAddressForOrder CustomerAddressForOrder { get; set; }
        public bool FirstNameDisabled { get; set; }
        public bool LastNameDisabled { get; set; }
        public bool PhoneNumberDisabled { get; set; }
        public bool ZipPostalCodeDisabled { get; set; }
        public bool AreaDisabled { get; set; }
        public bool RegionDisabled { get; set; }
        public bool CityDisabled { get; set; }
        public bool StreetDisabled { get; set; }
        public bool HouseDisabled { get; set; }
        public bool FlatDisabled { get; set; }
    }
}