namespace Nop.Plugin.Shipping.NovaPoshta
{
    public static class NovaPoshtaDefaults
    {
        public const string UPDATE_DATA_TASK_TYPE = "Nop.Plugin.Shipping.NovaPoshta.Services.NovaPoshtaUpdateScheduledTask";
        public const int DEFAULT_SYNCHRONIZATION_PERIOD = 24;
        public const string SYNCHRONIZATION_TASK_NAME = "Update data (Nova Poshta plugin)";
        
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
    }
}