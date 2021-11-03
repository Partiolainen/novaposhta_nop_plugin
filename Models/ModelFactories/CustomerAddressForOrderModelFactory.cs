using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta.Models.ModelFactories
{
    public class CustomerAddressForOrderModelFactory
    {
        public CustomerAddressForOrderPartialViewModel BuildPartialViewModel(NpCustomerAddressForOrder customerAddressForOrder,
            bool disabledByDefault = false)
        {
            var customerAddressForOrderModel = new CustomerAddressForOrderPartialViewModel(customerAddressForOrder)
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

        public NpCustomerAddressForOrderModel BuildModel(NpCustomerAddressForOrder customerAddressForOrder)
        {
            return new NpCustomerAddressForOrderModel
            {
                Id = customerAddressForOrder.Id,
                OrderId = customerAddressForOrder.OrderId,
                ZipPostalCode = customerAddressForOrder.ZipPostalCode,
                Area = customerAddressForOrder.Area,
                Region = customerAddressForOrder.Region,
                City = customerAddressForOrder.City,
                Street = customerAddressForOrder.Street,
                House = customerAddressForOrder.House,
                Flat = customerAddressForOrder.Flat,
                FirstName = customerAddressForOrder.FirstName,
                LastName = customerAddressForOrder.LastName,
                PhoneNumber = customerAddressForOrder.PhoneNumber,
            };
        }

        public NpCustomerAddressForOrder BuildAddress(NpCustomerAddressForOrderModel customerAddressForOrderModel)
        {
            return new NpCustomerAddressForOrder
            {
                Id = customerAddressForOrderModel.Id,
                OrderId = customerAddressForOrderModel.OrderId,
                ZipPostalCode = customerAddressForOrderModel.ZipPostalCode,
                Area = customerAddressForOrderModel.Area,
                Region = customerAddressForOrderModel.Region,
                City = customerAddressForOrderModel.City,
                Street = customerAddressForOrderModel.Street,
                House = customerAddressForOrderModel.House,
                Flat = customerAddressForOrderModel.Flat,
                FirstName = customerAddressForOrderModel.FirstName,
                LastName = customerAddressForOrderModel.LastName,
                PhoneNumber = customerAddressForOrderModel.PhoneNumber,
            };
        }
    }
}