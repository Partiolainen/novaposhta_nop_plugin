using Nop.Plugin.Shipping.NovaPoshta.Domain;

namespace Nop.Plugin.Shipping.NovaPoshta.Services.Factories
{
    public class NpCustomerAddressForOrderFactory
    {
        public static NpCustomerAddressForOrder FromNpSettlement(NovaPoshtaSettlement novaPoshtaSettlement)
        {
            var npCustomerAddressForOrder = new NpCustomerAddressForOrder
            {
                ZipPostalCode = novaPoshtaSettlement.Index1,
                Area = novaPoshtaSettlement.AreaDescription,
                Region = novaPoshtaSettlement.RegionsDescription,
                City = novaPoshtaSettlement.Description,
            };

            return npCustomerAddressForOrder;
        }
    }
}