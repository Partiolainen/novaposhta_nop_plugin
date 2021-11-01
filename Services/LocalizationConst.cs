using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Shipping.NovaPoshta.Services
{
    public static class LocalizationConst
    {
        public const string SHIPPING_METHOD_NAME = "Plugins.Shipping.NovaPoshta.views.shippingMethodName";
        public const string SHIPPING_METHOD_TO_WAREHOUSE = "Plugins.Shipping.NovaPoshta.views.shippingMethodToWarehouse";
        public const string SHIPPING_METHOD_ADDRESS = "Plugins.Shipping.NovaPoshta.views.shippingMethodAddress";
        public const string API_KEY = "Plugins.Shipping.NovaPoshta.Fields.ApiKey";
        public const string API_URL = "Plugins.Shipping.NovaPoshta.Fields.ApiUrl";
        public const string USE_ADDITIONAL_FEE = "Plugins.Shipping.NovaPoshta.Fields.UseAdditionalFee";
        public const string ADDITIONAL_FEE_IS_PERCENT = "Plugins.Shipping.NovaPoshta.Fields.AdditionalFeeIsPercent";
        public const string ADDITIONAL_FEE = "Plugins.Shipping.NovaPoshta.Fields.AdditionalFee";
        public const string DB_LAST_SUCCESS_UPDATE = "Plugins.Shipping.NovaPoshta.Fields.DbLastSuccessUpdate";
        public const string WAREHOUSE_CITIES = "Plugins.Shipping.NovaPoshta.Fields.WarehouseCities";
        public const string WAREHOUSE_UNAVAILABLE_MESSAGE = "Plugins.Shipping.NovaPoshta.Fields.WarehouseUnavailableMessge";
        public const string SHIPPING_DETAILS = "Plugins.Shipping.NovaPoshta.Fields.ShippingDetails";
        public const string SHIPPING_POINT_DETAILS = "Plugins.Shipping.NovaPoshta.Fields.ShippingPointDetails";
        public const string SHIPPING_POINT_NUMBER = "Plugins.Shipping.NovaPoshta.Fields.ShippingPointNumber";
        public const string SHIPPING_POINT = "Plugins.Shipping.NovaPoshta.Fields.ShippingPoint";
        public const string CHANGE_SHIPPING_POINT = "Plugins.Shipping.NovaPoshta.Fields.ChangeShippingPoint";
        public const string CREATE_SHIPMENT_WAYBILL = "Plugins.Shipping.NovaPoshta.Fields.CreateShipmentWaybill";
        
        public static IList<string> GetValues()
        {
            return typeof(LocalizationConst).GetFields().Select(fieldInfo => fieldInfo.Name).ToList();
        }
    }
}