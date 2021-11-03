using Nop.Plugin.Shipping.NovaPoshta.Domain;
using Nop.Plugin.Shipping.NovaPoshta.Infrastructure.GenericAttributes;

namespace Nop.Plugin.Shipping.NovaPoshta.Models
{
    public class ShippingToWarehouseInfoModel
    {
        public NovaPoshtaWarehouse NovaPoshtaWarehouse { get; set; }
        public ToWarehouseCustomerMainInfo ToWarehouseCustomerMainInfo { get; set; }

        public ShippingToWarehouseInfoModel()
        {
        }

        public ShippingToWarehouseInfoModel(NovaPoshtaWarehouse novaPoshtaWarehouse, ToWarehouseCustomerMainInfo toWarehouseCustomerMainInfo)
        {
            NovaPoshtaWarehouse = novaPoshtaWarehouse;
            ToWarehouseCustomerMainInfo = toWarehouseCustomerMainInfo;
        }
    }
}