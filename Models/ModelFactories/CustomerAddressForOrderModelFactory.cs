using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta.Models.ModelFactories
{
    public class CustomerAddressForOrderModelFactory
    {
        public CustomerAddressForOrderModel BuildModel(NpCustomerAddressForOrder customerAddressForOrder,
            bool disabledByDefault = false)
        {
            var customerAddressForOrderModel = new CustomerAddressForOrderModel(customerAddressForOrder)
                {
                    ZipPostalCodeDisabled = disabledByDefault,
                    AreaDisabled = disabledByDefault,
                    RegionDisabled = disabledByDefault,
                    CityDisabled = disabledByDefault,
                    StreetDisabled = disabledByDefault,
                    HouseDisabled = disabledByDefault,
                    FlatDisabled = disabledByDefault,
                    FirstNameDisabled = disabledByDefault,
                    LastNameDisabled = disabledByDefault,
                    PhoneNumberDisabled = disabledByDefault
                };

            return customerAddressForOrderModel;
        }
    }
}